
// This file has been generated by the GUI designer. Do not modify.
namespace Wallet
{
	public partial class MenuButton
	{
		private global::Gtk.EventBox eventbox;

		private global::Gtk.VBox vbox9;

		private global::Gtk.HBox hbox1;

		protected virtual void Build()
		{
			global::Stetic.Gui.Initialize(this);
			// Widget Wallet.MenuButton
			global::Stetic.BinContainer.Attach(this);
			this.Name = "Wallet.MenuButton";
			// Container child Wallet.MenuButton.Gtk.Container+ContainerChild
			this.eventbox = new global::Gtk.EventBox();
			this.eventbox.Name = "eventbox";
			// Container child eventbox.Gtk.Container+ContainerChild
			this.vbox9 = new global::Gtk.VBox();
			this.vbox9.Name = "vbox9";
			this.vbox9.Homogeneous = true;
			this.vbox9.Spacing = 6;
			// Container child vbox9.Gtk.Box+BoxChild
			this.hbox1 = new global::Gtk.HBox();
			this.hbox1.Name = "hbox1";
			this.hbox1.Spacing = 6;
			this.vbox9.Add(this.hbox1);
			global::Gtk.Box.BoxChild w1 = ((global::Gtk.Box.BoxChild)(this.vbox9[this.hbox1]));
			w1.Position = 0;
			this.eventbox.Add(this.vbox9);
			this.Add(this.eventbox);
			if ((this.Child != null))
			{
				this.Child.ShowAll();
			}
			this.Hide();
		}
	}
}
