package md561d37b88b374f1f338636cdc9354c2a2;


public class Program
	extends md5cedead65730cfb9c4b33fbfd5914d87f.AndroidGameActivity
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onCreate:(Landroid/os/Bundle;)V:GetOnCreate_Landroid_os_Bundle_Handler\n" +
			"";
		mono.android.Runtime.register ("Tiled.Droid.Program, Tiled.Droid, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", Program.class, __md_methods);
	}


	public Program ()
	{
		super ();
		if (getClass () == Program.class)
			mono.android.TypeManager.Activate ("Tiled.Droid.Program, Tiled.Droid, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}


	public void onCreate (android.os.Bundle p0)
	{
		n_onCreate (p0);
	}

	private native void n_onCreate (android.os.Bundle p0);

	private java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}
