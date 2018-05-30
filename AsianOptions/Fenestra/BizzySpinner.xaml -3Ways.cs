// Copyright © Microsoft Corporation 2009
// Distrubted under the Microsoft Permissive License 
//


//efine USE_SIMPLE_ANIMATION
//efine USE_STORY_BOARD
#define USE_CHAINED_ANIMATIONS

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Markup;
using System.ComponentModel;

namespace Fenestra
{
    /// <summary>A spinning busy state indicator.</summary>
    public class BizzySpinner : ContentControl
    {
        //--------------------------------------------------------------------------------------------------------------------------
        #region Construction

        static BizzySpinner()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(BizzySpinner), new FrameworkPropertyMetadata(typeof(BizzySpinner)));
        }

        public BizzySpinner()
            : base()
        {
            IsEnabledChanged += IsEnabledChangedHandler;
        }

        #endregion
        //--------------------------------------------------------------------------------------------------------------------------
        #region Events

        /// <summary>Raised when the spin property is changed.</summary>
        /// <remarks>Note, the control will only spin when Spin is true and the control is enabled.</remarks>
        public event DependencyPropertyChangedEventHandler IsSpinChanged;

        /// <summary>Raised when the spinning property is changed.</summary>
        /// <remarks>Note, the control will only spin when Spin is true and the control is enabled.</remarks>
        public event DependencyPropertyChangedEventHandler IsSpinningChanged;

        #endregion
        //--------------------------------------------------------------------------------------------------------------------------
        #region Enbable State Change Handler

        private Brush BackgroundBrushSave;
        private Brush LeaderBrushSave;
        private Brush TailBrushSave;

        /// <summary>Handles the state change between the enabled and disabled states.</summary>
        private void IsEnabledChangedHandler(Object sender, DependencyPropertyChangedEventArgs e)
        {
            if (((bool)e.OldValue) && !((bool)e.NewValue))
            {
                //
                // Going disabled
                //
                BackgroundBrushSave = Background;
                LeaderBrushSave = LeaderBrush;
                TailBrushSave = TailBrush;

                Background = DisabledBackgroundBrush;
                LeaderBrush = DisabledLeaderBrush;
                TailBrush = DisabledTailBrush;

                ControlSpinning(false);
            }

            if (!((bool)e.OldValue) && ((bool)e.NewValue))
            {
                //
                // Going enabled
                //
                Background = BackgroundBrushSave;
                LeaderBrush = LeaderBrushSave;
                TailBrush = TailBrushSave;

                // The control is enabled, turn on spinning if the Spin property is ture
                ControlSpinning(Spin);
            }
        }

        #endregion
        //--------------------------------------------------------------------------------------------------------------------------
        #region Dependancy Properties
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        #region Angle Dependancy Property

        public static readonly DependencyProperty AngleProperty = DependencyProperty.Register(
            "Angle",                                                   // The name of the dependency property to register
            typeof(double),                                            // The type of the property
            typeof(BizzySpinner),                                      // The owner type that is registering the dependency property.
            new FrameworkPropertyMetadata(                             // Property metadata for the dependency property;;
                0.0,                                                   // default value
                new PropertyChangedCallback(OnAnglePropertyChanged),
                new CoerceValueCallback(AngleCoerceCallback)
                )
            );

        /// <summary>The angle of the control.  Only active when Spin is false.  Used to manual control the angle of the spinner.</summary>
        [Category("Behavior")]
        public double Angle
        {
            get { return (double)GetValue(AngleProperty); }
            set { SetValue(AngleProperty, value); }
        }

        private static void OnAnglePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            BizzySpinner me = (BizzySpinner)d;

            if (!me.Spin && me.IsEnabled) {
                me.SpinAngle = (double)e.NewValue;
            }
        }

        private static Object AngleCoerceCallback(DependencyObject d, Object baseValue)
        {
            if (!(baseValue is double)) {
                return DependencyProperty.UnsetValue;
            }

            double b = (double)baseValue;

            double v = b % 360.0;

            return v;
        }

        #endregion
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        #region Spin Dependancy Property

        public static readonly DependencyProperty SpinProperty = DependencyProperty.Register(
            "Spin",                             // The name of the dependency property to register
            typeof(bool),                       // The type of the property
            typeof(BizzySpinner),               // The owner type that is registering the dependency property.
            new FrameworkPropertyMetadata(      // Property metadata for the dependency property
                false,
                new PropertyChangedCallback(OnSpinPropertyChanged)
                )
            );

        /// <summary>When true the control spins.  Note, the control does not spin when disabled.</summary>
        [Category("Behavior")]
        public bool Spin
        {
            get { return (bool)GetValue(SpinProperty); }
            set { SetValue(SpinProperty, value); }
        }

        private static void OnSpinPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            BizzySpinner me = (BizzySpinner)d;

            if ((bool)e.NewValue)
            {
                // Spin is true: turn spin on if this control is enabled
                me.ControlSpinning(me.IsEnabled);
            }

            if ( !(bool)e.NewValue)
            {
                me.ControlSpinning(false);
            }

            if ( me.IsSpinChanged != null ) {
                me.IsSpinChanged(me, e);
            }
        }

        #endregion 
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        #region Spinning Dependancy Property

        public static readonly DependencyPropertyKey SpinningPropertyKey = DependencyProperty.RegisterReadOnly(
            "Spinning",                         // The name of the dependency property to register
            typeof(bool),                       // The type of the property
            typeof(BizzySpinner),               // The owner type that is registering the dependency property.
            new FrameworkPropertyMetadata(      // Property metadata for the dependency property
                false,
                new PropertyChangedCallback(OnSpinningPropertyChanged)
                )
            );

        /// <summary>True when the control is spinning.</summary>
        [Category("Common")]
        public bool Spinning
        {
            get { return (bool)GetValue(SpinningPropertyKey.DependencyProperty); }
        }

        private static void OnSpinningPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            BizzySpinner me = (BizzySpinner)d;

            if (me.IsSpinningChanged != null) {
                me.IsSpinningChanged(me, e);
            }
        }

        #endregion 
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        #region SpinRate Dependancy Property

        public static readonly DependencyProperty SpinRateProperty = DependencyProperty.Register(
            "SpinRate",                         // The name of the dependency property to register
            typeof(double),                     // The type of the property
            typeof(BizzySpinner),               // The owner type that is registering the dependency property.
            new FrameworkPropertyMetadata(      // Property metadata for the dependency property
                1.0,                                                     // default 1 second
                FrameworkPropertyMetadataOptions.None,
                new PropertyChangedCallback(OnSpinRatePropertyChanged),
                new CoerceValueCallback(SpinRateCoerceCallback)
                )
            );

        /// <summary>The rotation period.  The control will make one rotation in this period of time.</summary>
        [Category("Behavior")]
        public double SpinRate
        {
            get { return (double)GetValue(SpinRateProperty); }
            set { SetValue(SpinRateProperty, value); }
        }

        private static void OnSpinRatePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            BizzySpinner me = (BizzySpinner)d;

            if (me.Spinning) {
                if (me.spinAnimation != null) {

                    Duration duration = TimeSpan.FromSeconds((double)e.NewValue);

                    if (me.spinAnimation.Duration != duration) {

                        me.spinAnimation.Duration = duration;

                        me.BeginSpinningAnimation();
                    }
                }
            }
        }

        private static Object SpinRateCoerceCallback(DependencyObject d, Object baseValue)
        {
            if (!(baseValue is double) )
            {
                return DependencyProperty.UnsetValue;
            }

            double v = (double)baseValue;

            if (v <= 0.0)
            {
                return DependencyProperty.UnsetValue;
            }

            return v;
        }

        #endregion
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        #region SpinAngle Dependancy Property

        public static readonly DependencyProperty SpinAngleProperty = DependencyProperty.Register(
            "SpinAngle",                               // The name of the dependency property to register
            typeof(double),                            // The type of the property
            typeof(BizzySpinner),                      // The owner type that is registering the dependency property.
            new FrameworkPropertyMetadata(             // Property metadata for the dependency property;;
                0.0 
                )
            );

        /// <summary>The current angle of of the control.</summary>
        /// <remarks>Template componets attach to ths property to spin.</remarks>
        [Category("Behavior")]
        public double SpinAngle
        {
            get { return (double)GetValue(SpinAngleProperty); }
            private set { SetValue(SpinAngleProperty, value); }
        }

        #endregion
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        #region SpinDirection Dependancy Property

        public static readonly DependencyProperty SpinDirectionProperty = DependencyProperty.Register(
            "SpinDirection",                           // The name of the dependency property to register
            typeof(BizzySpinnerDirection),             // The type of the property
            typeof(BizzySpinner),                      // The owner type that is registering the dependency property.
            new FrameworkPropertyMetadata(             // Property metadata for the dependency property;;
                BizzySpinnerDirection.Clockwise
                )
            );

        ///<summary>Gets or sets the spin direction.</summary>
        [Category("Behavior")]
        public BizzySpinnerDirection SpinDirection
        {
            get { return (BizzySpinnerDirection)GetValue(SpinDirectionProperty); }
            set { SetValue(SpinDirectionProperty, value); }
        }

        #endregion
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        #region LeaderBrush Dependancy Property

        public static readonly DependencyProperty LeaderBrushProperty = DependencyProperty.Register(
            "LeaderBrush",                      // The name of the dependency property to register
            typeof(Brush),                      // The type of the property
            typeof(BizzySpinner),               // The owner type that is registering the dependency property.
            new FrameworkPropertyMetadata(      // Property metadata for the dependency property;;
                new SolidColorBrush( Colors.Red )
                )
            );

        ///<summary>The brush for the leading spinner shape.</summary>
        [Category("Brushes")]
        public Brush LeaderBrush
        {
            get { return (Brush)GetValue(LeaderBrushProperty); }
            set { SetValue(LeaderBrushProperty, value); }
        }

        #endregion
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        #region TailBrush Dependancy Property

        public static readonly DependencyProperty TailBrushProperty = DependencyProperty.Register(
            "TailBrush",                      // The name of the dependency property to register
            typeof(Brush),                      // The type of the property
            typeof(BizzySpinner),               // The owner type that is registering the dependency property.
            new FrameworkPropertyMetadata(      // Property metadata for the dependency property;;
                new SolidColorBrush(Colors.Green)
                )
            );

        ///<summary>The brush for the Trailing spinner shapes.</summary>
        [Category("Brushes")]
        public Brush TailBrush
        {
            get { return (Brush)GetValue(TailBrushProperty); }
            set { SetValue(TailBrushProperty, value); }
        }

        #endregion 
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        #region DisabledLeaderBrush Dependancy Property

        public static readonly DependencyProperty DisabledLeaderBrushProperty = DependencyProperty.Register(
            "DisabledLeaderBrush",                       // The name of the dependency property to register
            typeof(Brush),                               // The type of the property
            typeof(BizzySpinner),                        // The owner type that is registering the dependency property.
            new FrameworkPropertyMetadata(               // Property metadata for the dependency property;;
                new SolidColorBrush( Colors.DarkGray )   // Default value
                )
            );

        ///<summary>The brush for the leading spinner shape when in the disabled state.</summary>
        [Category("Brushes")]
        public Brush DisabledLeaderBrush
        {
            get { return (Brush)GetValue(DisabledLeaderBrushProperty); }
            set { SetValue(DisabledLeaderBrushProperty, value); }
        }

        #endregion
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        #region DisabledTailBrush Dependancy Property

        public static readonly DependencyProperty DisabledTailBrushProperty = DependencyProperty.Register(
            "DisabledTailBrush",                        // The name of the dependency property to register
            typeof(Brush),                              // The type of the property
            typeof(BizzySpinner),                       // The owner type that is registering the dependency property.
            new FrameworkPropertyMetadata(              // Property metadata for the dependency property;;
                new SolidColorBrush(Colors.LightGray)   // Default value
                )
            );

        ///<summary>The brush for the trailing spinner shapes when in the disabled state.</summary>
        [Category("Brushes")]
        public Brush DisabledTailBrush
        {
            get { return (Brush)GetValue(DisabledTailBrushProperty); }
            set { SetValue(DisabledTailBrushProperty, value); }
        }

        #endregion
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        #region DisabledBackgroundBrush Dependancy Property

        public static readonly DependencyProperty DisabledBackgroundBrushProperty = DependencyProperty.Register(
            "DisabledBackgroundBrush",                  // The name of the dependency property to register
            typeof(Brush),                              // The type of the property
            typeof(BizzySpinner),                       // The owner type that is registering the dependency property.
            new FrameworkPropertyMetadata(              // Property metadata for the dependency property;;
                null                                    // Default value
                )
            );

        ///<summary>The brush for the control when in the disabled state.</summary>
        [Category("Brushes")]
        public Brush DisabledBackgroundBrush
        {
            get { return (Brush)GetValue(DisabledBackgroundBrushProperty); }
            set { SetValue(DisabledBackgroundBrushProperty, value); }
        }

        #endregion
        #endregion 
        //--------------------------------------------------------------------------------------------------------------------------
        #region Animation Control

        private DoubleAnimation spinAnimation = null;

        private double OneRotation
        {
            get
            {
                double direction = (SpinDirection == BizzySpinnerDirection.Clockwise) ? 1 : -1;
                return 360.0 * direction;
            }
        }

#if USE_STORY_BOARD

        private Storyboard storyboard = null;

        ///<summary>Begins, or re-begins the spin animation.</summary>
        void ReBeginSpinningAnimation()
        {
            spinAnimation.From = SpinAngle;
            spinAnimation.To = SpinAngle + OneRotation;

            storyboard.Begin(this, true);
        }

        /// <summary>Turns Spinning on and off</summary>
        /// <param name="shouldSpin">True to enable spinning</param>
        void ControlSpinning(bool shouldSpin)
        {
            if (shouldSpin)
            {
                if (!Spinning)
                {
                    if (spinAnimation != null)
                    {
                        ReBeginSpinningAnimation();
                    }
                    else
                    {
                        INameScope nameScope = NameScope.GetNameScope(this);

                        if ( nameScope == null)
                        {
                            nameScope = new NameScope();
                            NameScope.SetNameScope(this, nameScope);
                        }
                        
                        // Create a new animation and storyboard

                        SpinAngle = Angle;

                        spinAnimation = new DoubleAnimation();
                        spinAnimation.Name = "BizzySpinnerAnimation";
                        spinAnimation.Duration = new Duration(TimeSpan.FromSeconds(SpinRate));
                        spinAnimation.RepeatBehavior = RepeatBehavior.Forever;

                        storyboard = new Storyboard();

                        NameScope.SetNameScope(storyboard, nameScope);

                        this.RegisterName("Puppet", this); // name this object 'Puppte' in the NameScope.

                        // Tie the animation to this spinner and the SpinAngleProperty
                        Storyboard.SetTargetName(spinAnimation, "Puppet");
                        Storyboard.SetTargetProperty(spinAnimation, new PropertyPath(BizzySpinner.SpinAngleProperty));

                        storyboard.Children.Add(spinAnimation);

                        ReBeginSpinningAnimation();
                    }

                    SetValue(SpinningPropertyKey, true); // Spinning = true;
                }
            }
            else
            {
                if (spinAnimation != null)
                {
                    if (Spinning)
                    {
                        //
                        // We have to remember the current spin angle.  Why?  Becuase the animation will
                        // reset the SpinAngleProperty back to its base value when it is removed
                        // from the SpinAngleProperty.  The SpinAngleProperty base value is the 
                        // animations from value. 
                        //
                        double tmp = SpinAngle % OneRotation;

                        storyboard.Stop(this);

                        Angle = tmp;
                        SpinAngle = tmp;

                        SetValue(SpinningPropertyKey, false); 
                    }
                }
            }
        }

#endif
#if USE_SIMPLE_ANIMATION

        ///<summary>Begins, or re-begins the spin animation.</summary>
        void ReBeginSpinningAnimation()
        {
            spinAnimation.Duration = new Duration(TimeSpan.FromSeconds(SpinRate));
            spinAnimation.From = SpinAngle;
            spinAnimation.To = SpinAngle + OneRotation; 

            //
            // From the MSDN documenation on Storyboard.Pause(): Calling the Begin method again 
            // replaces the paused storyboard with a new one, which has the appearance of resuming it. 
            //
            // From the MSND documention on Storybaord.begin(): If the targeted properties are already animated, 
            // they are replaced using the SnapshotAndReplace handoff behavior. 
            //
            this.BeginAnimation(SpinAngleProperty, spinAnimation);
        }

        /// <summary>Turns Spinning on and off</summary>
        /// <param name="shouldSpin">True to enable spinning</param>
        void ControlSpinning(bool shouldSpin)
        {
            if (shouldSpin) {
                if (!Spinning) {
                    if (spinAnimation != null) {

                        this.BeginAnimation(SpinAngleProperty, spinAnimation);

                    } else {
                        // Create a new animation 

                        SpinAngle = Angle;

                        spinAnimation = new DoubleAnimation();
                        spinAnimation.RepeatBehavior = RepeatBehavior.Forever;

                        ReBeginSpinningAnimation();
                    }

                    SetValue(SpinningPropertyKey, true); // Spinning = true;
                }
            } else {
                if (spinAnimation != null) {
                    if (Spinning)
                    {
                        //
                        // We have to remember the current spin angle.  Why?  Becuase the animation will
                        // reset the SpinAngleProperty back to its base value when it is removed
                        // from the SpinAngleProperty.  The SpinAngleProperty base value is the 
                        // animations from value. 
                        //
                        double tmp = SpinAngle % OneRotation;

                        this.BeginAnimation(SpinAngleProperty, null);

                        Angle = tmp;
                        SpinAngle = tmp;

                        SetValue(SpinningPropertyKey, false); 

                        //
                        // We have to 'forget' the spin animation here because once set, it never
                        // forget's the SpinAngleProperty base value so we will get jerks when
                        // spin is renabled.  Said another way, simply removing the animation
                        // from the property does not cause it to forget its state.  So, we must
                        // throw away the current animation and create a new one next time
                        // spinning is re-enabled.
                        //
                        spinAnimation = null;
                    }
                }
            }
        }
#endif
#if USE_CHAINED_ANIMATIONS

        enum SpinStates { NotSpinning, Acclerating, Running, Decelerating };

        /// <summary>Turns Spinning on and off</summary>
        /// <param name="shouldSpin">True to enable spinning</param>
        void ControlSpinning(bool shouldSpin)
        {
            if (shouldSpin)
            {
                if (!Spinning)
                {

                    BeginSpinningAnimation();

                    SetValue(SpinningPropertyKey, true); // Spinning = true;
                }
            }
            else
            {
                if (spinAnimation != null)
                {
                    if (Spinning)
                    {
                        StopSpinningAnimation();
                    }
                }
            }
        }

        ///<summary>Begins, or re-begins the spin animation.</summary>
        void BeginSpinningAnimation()
        {
            SpinAngle = Angle;

            if (spinAnimation == null)
            {
                spinAnimation = new DoubleAnimation();
            } 

            spinAnimation.From = SpinAngle;
            spinAnimation.To = (SpinAngle + (OneRotation / 8));
            spinAnimation.Duration = new Duration(TimeSpan.FromSeconds(SpinRate/4));
            spinAnimation.AccelerationRatio = 1.0;

            spinAnimation.Completed += SpinContinuously;

            //
            // From the MSDN documenation on Storyboard.Pause(): Calling the Begin method again 
            // replaces the paused storyboard with a new one, which has the appearance of resuming it. 
            //
            // From the MSND documention on Storybaord.begin(): If the targeted properties are already animated, 
            // they are replaced using the SnapshotAndReplace handoff behavior. 
            //
            this.BeginAnimation(SpinAngleProperty, spinAnimation);
        }

        /// <summary>Updates the animation to spin continuously</summary>
        void SpinContinuously(object sender, EventArgs e)
        {
            spinAnimation.Completed -= SpinContinuously;

            spinAnimation.From = SpinAngle;
            spinAnimation.To = SpinAngle + OneRotation;
            spinAnimation.Duration = new Duration(TimeSpan.FromSeconds(SpinRate));
            spinAnimation.AccelerationRatio = 0.0;
            spinAnimation.RepeatBehavior = RepeatBehavior.Forever;

            this.BeginAnimation(SpinAngleProperty, spinAnimation);
        }

        ///<summary>Begins, or re-begins the spin animation.</summary>
        void StopSpinningAnimation()
        {
            spinAnimation.From = SpinAngle;
            spinAnimation.To = (SpinAngle + (OneRotation / 8));
            spinAnimation.Duration = new Duration(TimeSpan.FromSeconds(SpinRate / 4));
            spinAnimation.DecelerationRatio = 1.0;
            spinAnimation.RepeatBehavior = new RepeatBehavior(1.0); // default: repeat once.

            spinAnimation.Completed += RemoveAnimation;

            this.BeginAnimation(SpinAngleProperty, spinAnimation);
        }

        /// <summary>Called when the animatino has slowed to a stop.  Removes the animation</summary>
        void RemoveAnimation(object sender, EventArgs e)
        {
            //
            // We have to remember the current spin angle becuase the animation will
            // reset the SpinAngleProperty back to its base value when it is removed
            // from the SpinAngleProperty.  The SpinAngleProperty base value is the 
            // animations from value. 
            //
            double tmp = SpinAngle % OneRotation;

            this.BeginAnimation(SpinAngleProperty, null);

            Angle = tmp;
            SpinAngle = tmp;

            SetValue(SpinningPropertyKey, false);

            //
            // We have to 'forget' the spin animation here because once set, it never
            // forget's the SpinAngleProperty base value so we will get jerks when
            // spin is renabled.  Said another way, simply removing the animation
            // from the property does not cause it to forget its state.  So, we must
            // throw away the current animation and create a new one next time
            // spinning is re-enabled.
            //
            spinAnimation = null;
        }

#endif

        #endregion
        //--------------------------------------------------------------------------------------------------------------------------
    }
}
