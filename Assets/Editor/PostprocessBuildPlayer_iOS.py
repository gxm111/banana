#!/usr/bin/python

#Add required libraries to Xcode project during build for Adcash
import sys

if sys.argv[2] != 'iPhone':
    sys.exit(0)

from mod_pbxproj import *
from os import path, listdir
from shutil import copytree

frameworks = [
              'AdSupport.framework',
              'MobileCoreServices.framework',
              'Security.framework',
              'CoreTelephony.framework',
              'SystemConfiguration.framework',
              'CoreGraphics.framework',
              'UIKit.framework'
              ]

framework_dir = path.join(sys.argv[1],'Frameworks','Plugins','iOS')



pbx_file_path = sys.argv[1] + '/Unity-iPhone.xcodeproj/project.pbxproj'
pbx_object = XcodeProject.Load(pbx_file_path)

pbx_object.add_framework_search_paths([path.abspath(framework_dir)])

for framework in frameworks:
    pbx_object.add_file('System/Library/Frameworks/' + framework, tree='SDKROOT')

pbx_object.save()
