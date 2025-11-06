using System.Runtime.InteropServices;
namespace Narula.File.NLock.Lib.UI;

internal static class InterOpUtil
{
	private const int WM_VSCROLL = 0x0115;
	private const int EM_GETSCROLLPOS = 0x0400 + 221;
	private const int WM_SETREDRAW = 0x000B;
	private const int SB_LINEUP = 0;
	private const int SB_LINEDOWN = 1;

	[StructLayout(LayoutKind.Sequential)]
	private struct POINT
	{
		public int X;
		public int Y;
	}

	[DllImport("user32.dll")]
	private static extern IntPtr SendMessage(IntPtr hWnd, int msg, int wParam, int lParam);

	[DllImport("user32.dll")]
	private static extern IntPtr SendMessage(IntPtr hWnd, int msg, int wParam, ref POINT pt);

	private static bool _probing = false;
	private static readonly object _lock = new object();
	public static bool IsScrolledToBottomProbe(this RichTextBox rtb)
	{
		lock (_lock)
		{
			if (_probing || rtb == null || !rtb.IsHandleCreated)
				return false;

			_probing = true;
			try
			{
				// get current scroll
				POINT before = new POINT();
				SendMessage(rtb.Handle, EM_GETSCROLLPOS, 0, ref before);

				// stop redraw to avoid flicker
				SendMessage(rtb.Handle, WM_SETREDRAW, 0, 0);

				// try to scroll down by one line
				SendMessage(rtb.Handle, WM_VSCROLL, SB_LINEDOWN, 0);

				// get new scroll
				POINT after = new POINT();
				SendMessage(rtb.Handle, EM_GETSCROLLPOS, 0, ref after);

				bool atBottom = after.Y == before.Y;

				// if we actually moved, move back up
				if (!atBottom)
				{
					SendMessage(rtb.Handle, WM_VSCROLL, SB_LINEUP, 0);
				}

				// resume redraw
				SendMessage(rtb.Handle, WM_SETREDRAW, 1, 0);
				rtb.Invalidate();

				return atBottom;
			}
			finally
			{
				_probing = false;
			}
		}
	}

}
