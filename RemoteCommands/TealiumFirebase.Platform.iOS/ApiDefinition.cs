using System;
using Foundation;
using Tealium.Platform.iOS;
using TealiumFirebase.Platform.iOS;

namespace TealiumFirebase.Platform.iOS
{
	// @interface FirebaseRemoteCommandWrapper
	interface FirebaseRemoteCommandWrapper
	{
		// -(instancetype _Nonnull)initWithType:(RemoteCommandTypeWrapper * _Nonnull)type __attribute__((objc_designated_initializer));
		[Export("initWithType:")]
		[DesignatedInitializer]
		IntPtr Constructor(RemoteCommandTypeWrapper type);
	}
}
