# ParticleButtonsApp
Call [particle.io](http://www.particle.io) device [Functions](https://docs.particle.io/reference/device-cloud/api/#call-a-function) with a simple button

This app allows you to crete multiple buttons to control your Photon, Code, etc devices from particle.io. You configure the Function name, arguments, token and your function will become available at the click of a button.

This App is published in the [Google Play store](https://play.google.com/store/apps/details?id=com.tiagonmas.ParticleButtons)
<a href="https://play.google.com/store/apps/details?id=com.tiagonmas.ParticleButtons"><img src="https://www.genetec.com/assets/Images/Global/Icons/Google-play-icon.png" width="50"></a>

# Issues using the app ?

If you have any issues using the app, please create a [new issue](https://github.com/tiagonmas/ParticleButtonsApp/issues)

# How to use the App

## First Run
When you first open the app, you're invited to create the first button. You need to click "Add" on the top of the screen, that will take you to the Button Settings screen.

## Button Settings
This is where you give the details of the button. It's name, and the details of the particle.io [function](https://docs.particle.io/reference/device-cloud/api/#call-a-function).

In the settings screen you have to fill in several fields:
1. Button Name - Whatver name you want to give to the button. The name will show on the main screen.
2. Function Name - The name of the function that will be called. As defined in your firmware code (eg: Particle.function("garage", garage));
3. Token - The Auth token to call the particle function. Check [docs](https://docs.particle.io/tutorials/device-cloud/authentication/#access-tokens)
4. Device ID - The id ID of your device that you want to call. Check your [console](https://console.particle.io/devices) for the Device Id.
5. Function Arguments - The arguments that are passed to the cloud function
6. Enabled - If the button is enabled in the main screen. If disabled the cloud funcion will not be called.
7. Order - to control how multiple buttons show up in the main screen. Lower numbers show first.

Example:
<br><img src="https://github.com/tiagonmas/ParticleButtons/blob/main/Docs/settings.png" width="400">


## Main screen
After you added your first button, the button will show up in the main screen.
<br>
<img src="https://github.com/tiagonmas/ParticleButtons/blob/main/Docs/mainpage.png" width="400">



# App Development
The source code is available in this repository. It was implemented using [Xamarin](https://dotnet.microsoft.com/apps/xamarin)

