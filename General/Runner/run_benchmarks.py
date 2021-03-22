import os
import shutil


cwd = os.getcwd()
test_file_path = os.path.join(cwd, "Test.txt")
gen_serialization_src_dir_path = os.path.join(cwd, "obj\\Release\\net6.0\\win-x64\\generated\\SerializationMechanismGenerator\\SerializationMechanismGenerator.SerializationMechanismGenerator")

helper_file_path = os.path.join(cwd, "Helper.cs")
buffer_writer_file_path = os.path.join(cwd, "PooledByteBufferWriter.cs")
mechanism_file_path = os.path.join(cwd, "SerializationMechanism.cs")
test_classes_file_path = os.path.join(cwd, "TestClasses.cs")

def clean_state():
	os.system('cmd /c "RMDIR /Q/S bin > nul 2>&1"')
	os.system('cmd /c "RMDIR /Q/S obj > nul 2>&1"')
	
	if os.path.exists(helper_file_path):
		os.remove(helper_file_path)
	if os.path.exists(buffer_writer_file_path):
		os.remove(buffer_writer_file_path)
	if os.path.exists(mechanism_file_path):
		os.remove(mechanism_file_path)
	if os.path.exists(test_classes_file_path):
		os.remove(test_classes_file_path)
	
	os.system('cmd /c "taskkill /F /IM dotnet.exe /T > nul 2>&1"')


def is_valid_specification(
	num_poco_option, 
	num_prop_option,
	use_attribute_option,
	process_option,
	processor_option):
	if num_poco_option != 1 and num_prop_option != 4:
		return False
					
	if use_attribute_option == 'true' and (processor_option == 'Reader' or processor_option == 'Writer'):
		return False

	if process_option == 'Read' and processor_option == 'Writer':
		return False

	if process_option == 'Write' and processor_option == 'Reader':
		return False

	return True


num_poco_options = [1, 10, 25, 50, 75, 100]
num_prop_options = [1, 4]
use_attribute_options = ['true', 'false']
process_options = ['Read', 'Write']
processor_options = ['DynamicSerializer', 'MetadataSerializer', 'Reader', 'Writer']

# num_poco_options = [1]
# num_prop_options = [1]
# use_attribute_options = ['false']
# process_options = ['Read']
# processor_options = ['Reader']

for num_poco_option in num_poco_options:
	for num_prop_option in num_prop_options:
		for use_attribute_option in use_attribute_options:
			for process_option in process_options:
				for processor_option in processor_options:
					if not is_valid_specification(
						num_poco_option, 
						num_prop_option,
						use_attribute_option,
						process_option,
						processor_option):
						continue

					# Write Test spec
					spec_content = "{}\n{}\n{}\n{}\n{}".format(num_poco_option, num_prop_option, use_attribute_option, process_option, processor_option)
					file = open(test_file_path, "w")
					file.write(spec_content)
					file.close()

					# Remove previous obj and bin dirs
					clean_state()

					# Generate serialization logic
					os.system('dotnet publish -c Release /p:UseMechanismGenerator=true')

					# Copy generated files to cwd
					gen_file_names = os.listdir(gen_serialization_src_dir_path)
					for file_name in gen_file_names:
						shutil.move(os.path.join(gen_serialization_src_dir_path, file_name), cwd)

					# Run benchmark and save result
					result_subdir_name = "-".join([
						"poco_{}".format(num_poco_option),
						"prop_{}".format(num_prop_option),
						"useattr_{}".format(use_attribute_option),
						"process_{}".format(process_option)
					])
					
					result_path = os.path.join(
						cwd,
						"Results",
						result_subdir_name,
						"{}.json".format(processor_option))

					run_cmd = 'crank --config Runner.yml \
						--scenario runner \
						--application.options.outputfiles C:\\Users\\laakinri.NORTHAMERICA\\Desktop\\SrcGenPerf\\System.Text.Json.dll \
						--application.buildarguments /p:RunningCrank=true \
						--application.buildarguments /p:PublishReadyToRun=true \
						--application.buildarguments /p:UseJsonGenerator=true \
						--profile remote-win \
						--script adjust_counters_win \
						--iterations 10 \
						--exclude 4 \
						--exclude-order load:wrk/rps/mean \
						--output {} \
					'.format(result_path)

					
					os.system(run_cmd)
