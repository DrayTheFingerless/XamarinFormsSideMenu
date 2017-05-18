using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace SmartSalesMA
{
	public class GlobalSideMenuLayout : RelativeLayout
	{
		private Frame _panel;
		private double _panelWidth = -1;

		public GlobalSideMenuLayout()
		{
			PanelShowing = true;

		}

		public void CreatePanel()
		{

			if (_panel == null)
			{
				var openButton = new Button() { HorizontalOptions = LayoutOptions.Start, 
                    Image = "ActionBarHomeBars.png",
					BackgroundColor = Color.Transparent,
					WidthRequest = 40
				};
				openButton.Clicked += (sender, e) => { 
                    PanelShowing = !PanelShowing;
					AnimatePanel();
				                //ChangeBackgroundColor();
				};

                _panel = new Frame()
                {
                    HasShadow = false,
                    OutlineColor = Global.GrayColor,
                    CornerRadius = 1,
                    Padding=0,

                    Content = new StackLayout()
                    {
                        Children = {
                        openButton,
                        ReturnSideMenu()
                        },
                        Padding = 0,
                        VerticalOptions = LayoutOptions.FillAndExpand,
                        HorizontalOptions = LayoutOptions.StartAndExpand,
                        BackgroundColor = Global.HighlightColor
                    }
                };

				Children.Add(_panel,
					Constraint.RelativeToParent((p) =>
					{
						return 0;
					}),
					Constraint.RelativeToParent((p) =>
					{
						return 0;
					}),
					Constraint.RelativeToParent((p) =>
					{
                        return 0;
					}),
					Constraint.RelativeToParent((p) =>
					{
						return p.Height;
					}));

				
			}

		}

		bool _PanelShowing = true;
		public bool PanelShowing
		{
		get { return _PanelShowing; }
		set { _PanelShowing = value; }
		}


		public async void AnimatePanel()
		{

			// swap the state
			//PanelShowing = !PanelShowing;

			// show or hide the panel
			if (this.PanelShowing)
			{
				// layout the panel to slide out
                var rect = new Rectangle(0, 0, (double)_panel.Parent.GetValue(View.WidthProperty)/3, this.Height);
				await this._panel.LayoutTo(rect, 10, Easing.Linear);
			}
			else
			{
				// layout the panel to slide in
				var rect = new Rectangle(0 , _panel.Y, 0, _panel.Height);
				await this._panel.LayoutTo(rect, 200, Easing.CubicOut);

				//// hide all children
				//foreach (var child in _panel.Chilsdren)
				//{
				//child.Scale = 0;
				//}
			}
		}


		/// <summary>
		/// Changes the background color of the relative layout
		/// </summary>
		public void ChangeBackgroundColor()
		{
			var repeatCount = 0;
			this.Animate(
				// set the name of the animation
				name: "changeBG",

				// create the animation object and callback
				animation: new Xamarin.Forms.Animation((val) =>
				{
							// val will be a from 0 - 1 and can use that to set a BG color
					if (repeatCount == 0)
						this.BackgroundColor = Color.FromRgb(1 - val, 1 - val, 1 - val);
					else
						this.BackgroundColor = Color.FromRgb(val, val, val);
				}),

				// set the length
				length: 750,

				// set the repeat action to update the repeatCount
				finished: (val, b) =>
				{
					repeatCount++;
				},

				// determine if we should repeat
				repeat: () =>
				{
					return repeatCount < 1;
				}
			);
		}

		public ListView ReturnSideMenu() { 
            
			var masterPageItems = new List<MasterPageItem>();
			masterPageItems.Add(new MasterPageItem
			{
				Title = "Home",
				//IconSource = "contacts.png",
				TargetType = typeof(DashboardPage)
			});
			masterPageItems.Add(new MasterPageItem
			{
				Title = "Contactos",
				//IconSource = "todo.png",
				TargetType = typeof(ContactosListPage)
			});
			masterPageItems.Add(new MasterPageItem
			{
				Title = "Visitas",
				//IconSource = "reminders.png",
				TargetType = typeof(SmartSalesMAPage)
			});
			masterPageItems.Add(new MasterPageItem
			{
				Title = "Encomendas",
				//IconSource = "reminders.png",
				TargetType = typeof(EncomendasListPage)
			});
			masterPageItems.Add(new MasterPageItem
			{
				Title = "Catálogo",
				//IconSource = "reminders.png",
				TargetType = typeof(CatalogoListPage)
			});

			var lista = new ListView()
				{
					ItemsSource = masterPageItems,
                 SeparatorColor = Global.GrayColor,
					ItemTemplate = new DataTemplate(() =>
					{
						var imageCell = new ImageCell();
						imageCell.SetBinding(TextCell.TextProperty, "Title");
                        imageCell.TextColor = Color.Black;
						return imageCell;
					}),

				};
			lista.ItemSelected += (sender, e) =>
				{
					var item = e.SelectedItem as MasterPageItem;
					if (item != null)
					{
						var insidepage = (Page)Activator.CreateInstance(item.TargetType);
						insidepage.Title = item.Title;
						Navigation.InsertPageBefore(insidepage, Navigation.NavigationStack.First());
						NavigationPage.SetBackButtonTitle(this, "");
						Navigation.PopToRootAsync(false);
					}
				};
		
			lista.VerticalOptions = LayoutOptions.FillAndExpand;
			return lista;
		}
	}
}

