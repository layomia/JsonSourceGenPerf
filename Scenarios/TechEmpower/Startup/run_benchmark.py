import os
import sys

def exec_cmd_quiet(cmd):
	os.system('cmd /c "{} > nul 2>&1"'.format(cmd))

def clean_state():
	exec_cmd_quiet('RMDIR /Q/S bin')
	exec_cmd_quiet('RMDIR /Q/S obj')

def parse_result_value(result_str):
	return int(result_str.split(':')[1])

def average(numbers):
	return sum(numbers) / len(numbers)

args = sys.argv

clean_state()

# Publish app
publish_cmd =  'cmd /c dotnet publish -c Release /p:Process={} /p:Processor={}'.format(args[1], args[2])
exec_cmd_quiet(publish_cmd)

# Run app
private_bytes = []
elasped_times = []

run_cmd = 'D:\\benchmarks\\JsonSourceGenPerf\\Startup\\bin\\Release\\net6.0\\win-x64\\publish\\Startup.exe'

for _ in range(4):
	results = os.popen(run_cmd).read().split('\n')
	private_bytes.append(parse_result_value(results[0]))
	elasped_times.append(parse_result_value(results[1]))

print('Private bytes (KB): {}'. format(average(private_bytes)))
print('Elapsed time (ms): {}'. format(average(elasped_times)))