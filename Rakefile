require 'bundler/setup'
require 'fileutils'

require 'albacore'
require 'albacore/tasks/versionizer'

Configuration = ENV['CONFIGURATION'] || 'Release'

Albacore::Tasks::Versionizer.new :versioning

task :clean do
  FileUtils.rmtree 'tests'
  FileUtils.rmtree 'build'
end

task :prepare_compile => [:clean] do
  FileUtils.cp 'src/app/Chaos.Portal.Authentication/App.config.sample', 'src/app/Chaos.Portal.Authentication/App.config'
  FileUtils.cp 'src/test/Chaos.Portal.Authentication.IntegrationTests/App.config.sample', 'src/test/Chaos.Portal.Authentication.IntegrationTests/App.config'
  FileUtils.cp 'src/test/Chaos.Portal.Authentication.Tests/App.config.sample', 'src/test/Chaos.Portal.Authentication.Tests/App.config'
end

desc 'Perform fast build (warn: doesn\'t d/l deps)'
build :quick_compile => [:prepare_compile] do |b|
  b.prop 'Configuration', Configuration
  b.logging = 'minimal'
  b.sln     = 'Authentication.sln'
end

task :package_tests => [:quick_compile] do
  FileUtils.mkdir 'tests'

  FileUtils.cp 'tools\NUnit-2.6.0.12051\bin\nunit.framework.dll', 'tests'
  FileUtils.cp 'src\app\Chaos.Portal.Authentication\bin\Release\Chaos.Portal.Authentication.dll', 'tests'
  FileUtils.cp 'tools\Moq.4.0.10827\NET40\Moq.dll', 'tests'
  FileUtils.cp 'lib\CHAOS.dll', 'tests'
  FileUtils.cp 'lib\Chaos.Portal.dll', 'tests'
  FileUtils.cp 'packages\Newtonsoft.Json.6.0.8\lib\net45\Newtonsoft.Json.dll', 'tests'
  FileUtils.cp 'packages\DotNetAuth.1.0.5\lib\net40\DotNetAuth.dll', 'tests'
  FileUtils.cp 'packages\DotNetAuth.Profiles.1.0.5\lib\net40\DotNetAuth.Profiles.dll', 'tests'

  FileUtils.cp 'src/test/Chaos.Portal.Authentication.Tests/WayfFilter.json', 'tests'
  
  system 'tools/ILMerge/ILMerge.exe',
    ['/out:tests\Chaos.Portal.Authentication.Test.dll',
     '/target:library',
     '/ndebug',
     '/lib:lib',
     '/targetplatform:v4,c:\windows\Microsoft.Net\Framework64\v4.0.30319',
     '/lib:c:\windows\Microsoft.Net\Framework64\v4.0.30319',
     'src\test\Chaos.Portal.Authentication.Tests\bin\Release\Chaos.Portal.Authentication.Tests.dll'], clr_command: true
end

desc "Run all the tests"
test_runner :tests => [:package_tests] do |tests|
  tests.files = FileList['tests/Chaos.Portal.Authentication.Test.dll']
  tests.add_parameter '/framework=4.0.30319'
  tests.exe = 'tools/NUnit-2.6.0.12051/bin/nunit-console.exe'
end

desc "Merges all production assemblies"
task :package => [:tests] do
  FileUtils.mkdir 'build'

  system 'tools/ILMerge/ILMerge.exe',
    ['/out:build\Chaos.Portal.Authentication.dll',
      '/target:library',
      '/ndebug',
      '/lib:lib',
      '/targetplatform:v4,c:\windows\Microsoft.Net\Framework64\v4.0.30319',
      '/lib:c:\windows\Microsoft.Net\Framework64\v4.0.30319',
      'src\app\Chaos.Portal.Authentication\bin\Release\Chaos.Portal.Authentication.dll'], clr_command: true
end

task :default => :package
