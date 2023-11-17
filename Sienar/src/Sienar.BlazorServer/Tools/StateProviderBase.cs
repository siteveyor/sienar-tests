using System;

namespace Sienar.BlazorServer.Tools;

public abstract class StateProviderBase
{
	public event Action? OnChange;

	protected void NotifyStateChanged() => OnChange?.Invoke();
}