#
# Be sure to run `pod lib lint 'TealiumFirebaseWrapper.podspec' to ensure this is a
# valid spec before submitting.
#
# Any lines starting with a # are optional, but their use is encouraged
# To learn more about a Podspec see https://guides.cocoapods.org/syntax/podspec.html
#

Pod::Spec.new do |s|
  s.name             = 'TealiumFirebaseWrapper'
  s.version          = '0.1.0'
  s.summary          = 'A short description of TealiumFirebaseWrapper.'

# This description is used to generate tags and improve search results.
#   * Think: What does it do? Why did you write it? What is the focus?
#   * Try to keep it short, snappy and to the point.
#   * Write the description between the DESC delimiters below.
#   * Finally, don't worry about the indent, CocoaPods strips it!

  s.description      = <<-DESC
TODO: Add long description of the pod here.
                       DESC

  s.static_framework = true

  s.homepage         = 'https://github.com/Enrico Zannini/TealiumFirebaseWrapper'
  # s.screenshots     = 'www.example.com/screenshots_1', 'www.example.com/screenshots_2'
  s.license          = { :type => 'MIT', :file => 'LICENSE' }
  s.author           = { 'Enrico Zannini' => 'enricozannini93@gmail.com' }
  s.source           = { :git => 'https://github.com/Enrico Zannini/TealiumFirebaseWrapper.git', :tag => s.version.to_s }
  # s.social_media_url = 'https://twitter.com/<TWITTER_USERNAME>'

  s.ios.deployment_target = '11.0'

  s.source_files = 'TealiumFirebaseWrapper/Classes/**/*'
  
  # s.resource_bundles = {
  #   'TealiumFirebaseWrapper' => ['TealiumFirebaseWrapper/Assets/*.png']
  # }

  # s.public_header_files = 'Pod/Classes/**/*.h'
  # s.frameworks = 'UIKit', 'MapKit'
  s.dependency 'TealiumWrapperCocoapods', '~> 0.1.1'
  s.dependency 'TealiumFirebase', '~> 2.1'
  
end
