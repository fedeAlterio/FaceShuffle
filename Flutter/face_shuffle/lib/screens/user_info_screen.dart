import 'package:face_shuffle/screens/style/colors.dart';
import 'package:flutter/material.dart';

class SelectUserInfoWidget extends StatefulWidget {
  const SelectUserInfoWidget({Key? key}) : super(key: key);

  @override
  _SelectUserInfoWidgetState createState() => _SelectUserInfoWidgetState();
}

class _SelectUserInfoWidgetState extends State<SelectUserInfoWidget>
    with TickerProviderStateMixin {
  late AnimationController _rotationAnimationController;
  late AnimationController _translationAnimationController;
  late AnimationController _fadeControlsInAnimationController;
  late AnimationController _zoomInAnimationController;
  late Animation<double> _translationAnimation;
  late Animation<double> _rotationAnimation;
  late Animation<double> _fadeControlsInAnimation;
  late Animation<double> _zoomInAnimation;
  final GlobalKey _centerKey = GlobalKey();
  final GlobalKey _faceKey = GlobalKey();
  final double faceLength = 170;

  @override
  void initState() {
    super.initState();

    var zoomInDuration = const Duration(seconds: 0, milliseconds: 500);
    _zoomInAnimationController = AnimationController(
      duration: zoomInDuration,
      vsync: this,
    );

    _zoomInAnimation = Tween<double>(
      begin: 0, // Initial offset outside the screen
      end: 1, // Final offset at the center of the screen
    ).animate(CurvedAnimation(
      parent: _zoomInAnimationController,
      curve: Curves.easeOutCubic,
    ));

    var translationDuration = const Duration(seconds: 1, milliseconds: 100);
    _rotationAnimationController = AnimationController(
      duration: translationDuration,
      vsync: this,
    );

    _rotationAnimation = Tween<double>(
      begin: 0, // Initial offset outside the screen
      end: 3.14 * 2, // Final offset at the center of the screen
    ).animate(CurvedAnimation(
      parent: _rotationAnimationController,
      curve: Curves.linear,
    ));

    _translationAnimationController =
        AnimationController(duration: translationDuration, vsync: this);

    _translationAnimation = Tween<double>(
      begin: 83 + faceLength / 2, // Initial offset outside the screen
      end: 0, // Final offset at the center of the screen
    ).animate(CurvedAnimation(
      parent: _translationAnimationController,
      curve: Curves.linear,
    ));

    _fadeControlsInAnimationController = AnimationController(
      duration: const Duration(milliseconds: 600),
      vsync: this,
    );

    _fadeControlsInAnimation = Tween<double>(
      begin: 0, // Initial offset outside the screen
      end: 1, // Final offset at the center of the screen
    ).animate(CurvedAnimation(
      parent: _fadeControlsInAnimationController,
      curve: Curves.easeInOut,
    ));
    Future.delayed(Duration(milliseconds: 1500)).then((value) {
      _zoomInAnimationController.forward().then((value) {
        Future.delayed(Duration(milliseconds: 300)).then((value) {
          var rotationTask = _rotationAnimationController.forward();
          var translationTask = _translationAnimationController.forward();
          Future.delayed(Duration(
                  microseconds:
                      (translationDuration.inMicroseconds * 0.65).round()))
              .then((value) => _fadeControlsInAnimationController.forward());
        });
      });
    });
  }

  @override
  void dispose() {
    _rotationAnimationController.dispose();
    super.dispose();
  }

  Offset faceOffsetFromCenter() {
    if (_faceKey.currentContext == null) return const Offset(0, 0);
    var faceRenderBox =
        _faceKey.currentContext?.findRenderObject() as RenderBox;

    var faceOffset = faceRenderBox.localToGlobal(Offset.zero);

    return faceOffset;
  }

  @override
  Widget build(BuildContext context) {
    return Container(
      constraints: BoxConstraints(maxWidth: 100),
      child: Stack(
        children: [
          Column(
            mainAxisAlignment: MainAxisAlignment.center,
            crossAxisAlignment: CrossAxisAlignment.center,
            children: [
              Center(
                child: AnimatedBuilder(
                  animation: _translationAnimation,
                  builder: (context, child) => Transform.translate(
                    offset: Offset(0, _translationAnimation.value),
                    child: AnimatedBuilder(
                      animation: _rotationAnimationController,
                      builder: (context, child) {
                        return Transform(
                          transform: Matrix4.identity()
                            ..rotateY(_rotationAnimation.value),
                          origin: Offset(-(faceLength / 2), 0),
                          alignment: FractionalOffset.centerRight,
                          child: AnimatedBuilder(
                            animation: _zoomInAnimationController,
                            builder: (context, child) => Transform.scale(
                              scale: _zoomInAnimation.value,
                              child: Image.asset(
                                key: _faceKey,
                                "assets/Faccina.png",
                                width: faceLength,
                                height: faceLength,
                              ),
                            ),
                          ),
                        );
                      },
                    ),
                  ),
                ),
              ),
              AnimatedBuilder(
                animation: _fadeControlsInAnimationController,
                builder: (context, child) => Transform.translate(
                  offset: Offset(0, (1 - _fadeControlsInAnimation.value) * 15),
                  child: Opacity(
                    opacity: _fadeControlsInAnimation.value,
                    child: Padding(
                      padding: const EdgeInsets.symmetric(horizontal: 10),
                      child: SizedBox(
                        width: 500,
                        child: Column(
                          children: [
                            SizedBox(height: 40),
                            TextField(
                              decoration: InputDecoration(
                                contentPadding:
                                    EdgeInsets.symmetric(horizontal: 20),
                                filled: true,
                                fillColor: Colors.white70,
                                hintText: 'Username',
                                border: OutlineInputBorder(
                                    borderRadius: BorderRadius.circular(100)),
                              ),
                            ),
                            SizedBox(height: 20),
                            TextField(
                              decoration: InputDecoration(
                                contentPadding:
                                    EdgeInsets.symmetric(horizontal: 20),
                                filled: true,
                                fillColor: Colors.white70,
                                hintText: 'Name',
                                border: OutlineInputBorder(
                                    borderRadius: BorderRadius.circular(100)),
                              ),
                            ),
                            SizedBox(height: 20),
                            TextField(
                              decoration: InputDecoration(
                                contentPadding:
                                    EdgeInsets.symmetric(horizontal: 20),
                                filled: true,
                                fillColor: Colors.white70,
                                hintText: 'Age',
                                border: OutlineInputBorder(
                                    borderRadius: BorderRadius.circular(100)),
                              ),
                            ),
                            SizedBox(height: 30),
                            MaterialButton(
                              onPressed: () {
                                // Perform action when "Next" button is pressed
                              },
                              shape: RoundedRectangleBorder(
                                borderRadius: BorderRadius.circular(
                                    100.0), // Adjust the radius value as needed
                              ),
                              color: AppColors.shade1,
                              child: Padding(
                                padding: EdgeInsets.symmetric(
                                    horizontal: 30,
                                    vertical:
                                        10), // Adjust the padding value as needed
                                child: Text(
                                  'Next',
                                  style: TextStyle(
                                    color: Colors.white,
                                    fontSize: 16.0,
                                  ),
                                ),
                              ),
                            ),
                          ],
                        ),
                      ),
                    ),
                  ),
                ),
              ),
            ],
          ),
          Center(
              key: _centerKey,
              child: Container(
                width: 0,
                height: 0,
                color: Colors.blue,
              ))
        ],
      ),
      decoration: BoxDecoration(
        gradient: LinearGradient(
          begin: Alignment.topLeft,
          end: Alignment.bottomRight,
          colors: [
            AppColors.shade1,
            AppColors.shade2,
          ],
        ),
      ),
    );
  }
}
