using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;
using CoreGraphics;

namespace Palmipedo.iOS.Core
{
    public class ToolbarUtils
    {
        public event EventHandler OnBackTouchUpInside;

        private UIView _targetView;
        private UIView _notchColor;
        private UIView _toolbarView;

        private nfloat _yBase = 15;
        private nfloat _marginTop;
        private nfloat _heightBottomRect = 100;//40.0f;

        //header
        private UIButton _btnBack;
        private UILabel _lblTitle;
        private UIButton[] _buttons;

        public nfloat YToolbarButtons { get { return 5; } }

        public bool HideBackButton { get; set; }
        public nfloat YPositionEndToolbar { get { return Height + _marginTop; } }
        public nfloat Height { get { return 40; } }


        public ToolbarUtils(UIView targetView)
        {
            _targetView = targetView;
            if (UIDevice.CurrentDevice.CheckSystemVersion(11, 0))
            {
                var guide = _targetView.SafeAreaInsets;
                _marginTop = guide.Top;
            }
            else
            {
                _marginTop = 20;
            }
        }

        public UIView GenerateToolbar(string title, params UIButton[] buttons)
        {
            _buttons = buttons.Where(x => x != null).ToArray();

            nfloat leftMargin = 0;
            if (!HideBackButton)
            {
                _btnBack = new UIButton();
                //_btnBack.SetTitle("B", UIControlState.Normal);
                _btnBack.SetImage(UIImage.FromBundle("Icon_Back"), UIControlState.Normal);
                _btnBack.TouchUpInside += (sender, args) =>
                {
                    //NavigationController.PopViewController(true);
                    if (OnBackTouchUpInside != null)
                        OnBackTouchUpInside.Invoke(sender, args);
                };
                _btnBack.Frame = new CGRect(10, YToolbarButtons, 30, 30);

                leftMargin = _btnBack.Frame.X + _btnBack.Frame.Width;
                leftMargin += 10; //padding 
            }
            else
            {
                leftMargin = 10;
                foreach (var item in buttons)
                {
                    if (item.Frame.X <= 50)
                    {
                        leftMargin += item.Frame.X + item.Frame.Width;
                    }
                }
            }

            _lblTitle = new UILabel();
            _lblTitle.TextColor = UIColor.White;
            _lblTitle.Font = UIFont.SystemFontOfSize(14);
            _lblTitle.Text = title;
            _lblTitle.Frame = new CGRect(leftMargin, YToolbarButtons + 5, title.GetUIViewWithFromString(_lblTitle.Font), 20);

            _notchColor = new UIView(new CGRect(0, 0, _targetView.Bounds.Width, _marginTop + 1));
            _notchColor.BackgroundColor = Context.DefaultBlue;

            _toolbarView = new UIView(new CGRect(0, _marginTop, _targetView.Bounds.Width, Height));
            _toolbarView.BackgroundColor = Context.DefaultBlue;

            if (_btnBack != null)
                _toolbarView.AddSubview(_btnBack);

            _toolbarView.AddSubview(_lblTitle);

            _toolbarView.AddSubviews(buttons);

            _toolbarView.AutoresizingMask = UIViewAutoresizing.FlexibleDimensions;

            UIView allToolbar = new UIView(new CGRect(0, 0, _targetView.Bounds.Width, YPositionEndToolbar));
            allToolbar.AddSubview(_notchColor);
            allToolbar.AddSubview(_toolbarView);

            return allToolbar;
        }

        public void SetToolbarTitle(string title)
        {
            _lblTitle.Text = title;
            _lblTitle.Frame = new CGRect(_lblTitle.Frame.X, _lblTitle.Frame.Y, title.GetUIViewWithFromString(_lblTitle.Font), 20);
        }

        public void SetBackgroundColor(UIColor color)
        {
            _notchColor.BackgroundColor = color;
            _toolbarView.BackgroundColor = color;
        }

        public void RemoveFromSuperview()
        {
            if (_btnBack != null)
                _btnBack.RemoveFromSuperview();

            if (_lblTitle != null)
                _lblTitle.RemoveFromSuperview();

            if (_buttons != null && _buttons.Length > 0)
            {
                foreach (var item in _buttons)
                {
                    item.RemoveFromSuperview();
                }
            }
        }
    }
}