//  --------------------------------
//  <copyright file="CircularProgressBar.xaml.cs" company="None">
//  Copyright (c) Huy Pham. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.opensource.org/licenses/ms-pl.html
//  </copyright>
//  ---------------------------------

namespace Dynamics365CustomizingDownloader.Controls
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Threading;

    /// <summary>
    /// Interaction logic for CircularProgressBars
    /// </summary>
    public partial class CircularProgressBar
    {
        /// <summary>
        /// UIPropertyMetadata MinimumProperty
        /// </summary>
        public static readonly DependencyProperty MinimumProperty =
            DependencyProperty.Register("Minimum", typeof(int), typeof(CircularProgressBar), new UIPropertyMetadata(1));

        /// <summary>
        /// UIPropertyMetadata MaximumProperty
        /// </summary>
        public static readonly DependencyProperty MaximumProperty =
            DependencyProperty.Register("Maximum", typeof(int), typeof(CircularProgressBar), new UIPropertyMetadata(1));

        /// <summary>
        /// UIPropertyMetadata ValueProperty
        /// </summary>
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(int), typeof(CircularProgressBar), new UIPropertyMetadata(100));

        #region Fields

        /// <summary>
        /// Animation Timer
        /// </summary>
        private readonly DispatcherTimer animationTimer;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CircularProgressBar"/> class.
        /// </summary>
        public CircularProgressBar()
        {
            this.InitializeComponent();

            this.IsVisibleChanged += this.OnVisibleChanged;

            this.animationTimer = new DispatcherTimer(DispatcherPriority.ContextIdle, this.Dispatcher)
            {
                Interval = new TimeSpan(0, 0, 0, 0, 75)
            };
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the minimum.
        /// </summary>
        /// <value>The minimum.</value>
        public int Minimum
        {
            get { return (int)GetValue(MinimumProperty); }
            set { SetValue(MinimumProperty, value); }
        }

        /// <summary>
        /// Gets or sets the maximum.
        /// </summary>
        /// <value>The maximum.</value>
        public int Maximum
        {
            get { return (int)GetValue(MaximumProperty); }
            set { SetValue(MaximumProperty, value); }
        }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>The value.</value>
        public int Value
        {
            get { return (int)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        #endregion

        /// <summary>
        /// Sets the position.
        /// </summary>
        /// <param name="ellipse">The ellipse.</param>
        /// <param name="offset">The offset.</param>
        /// <param name="posOffSet">The position off set.</param>
        /// <param name="step">The step to change.</param>
        private static void SetPosition(DependencyObject ellipse, double offset, double posOffSet, double step)
        {
            ellipse.SetValue(Canvas.LeftProperty, 50 + (Math.Sin(offset + (posOffSet * step)) * 50));
            ellipse.SetValue(Canvas.TopProperty, 50 + (Math.Cos(offset + (posOffSet * step)) * 50));
        }

        /// <summary>
        /// Starts this instance.
        /// </summary>
        private void Start()
        {
            this.animationTimer.Tick += this.OnAnimationTick;
            this.animationTimer.Start();
        }

        /// <summary>
        /// Stops this instance.
        /// </summary>
        private void Stop()
        {
            this.animationTimer.Stop();
            this.animationTimer.Tick -= this.OnAnimationTick;
        }

        /// <summary>
        /// Handles the animation tick.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void OnAnimationTick(object sender, EventArgs e)
        {
            spinnerRotate.Angle = (spinnerRotate.Angle + 36) % 360;
        }

        /// <summary>
        /// Handles the loaded.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void OnCanvasLoaded(object sender, RoutedEventArgs e)
        {
            const double Offset = Math.PI;
            const double Step = Math.PI * 2 / 10.0;

            SetPosition(_circle0, Offset, 0.0, Step);
            SetPosition(_circle1, Offset, 1.0, Step);
            SetPosition(_circle2, Offset, 2.0, Step);
            SetPosition(_circle3, Offset, 3.0, Step);
            SetPosition(_circle4, Offset, 4.0, Step);
            SetPosition(_circle5, Offset, 5.0, Step);
            SetPosition(_circle6, Offset, 6.0, Step);
            SetPosition(_circle7, Offset, 7.0, Step);
            SetPosition(_circle8, Offset, 8.0, Step);
        }

        /// <summary>
        /// Handles the unloaded.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void OnCanvasUnloaded(object sender, RoutedEventArgs e)
        {
            this.Stop();
        }

        /// <summary>
        /// Handles the visible changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Windows.DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private void OnVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var isVisible = (bool)e.NewValue;

            if (isVisible)
            {
                this.Start();
            }
            else
            {
                this.Stop();
            }
        }
    }
}
