Changelog
=========

[1.0.0] - 2026.02.27
--------------------
* **Added** 
    * ICameraProvider.cs - It’s a contract for any component/service that can provide access to a Camera object.
    * FeatureCondition`s - It defines an abstract ScriptableObject base for feature‑toggle conditions.
    * HorizontalSelectorsLayout - This class arranges a horizontal list of UI elements inside a scrolling panel, sizing them to fit.
    * PageSelector - components for pagination.
    * RecoverableAds - A localized Unity component button based on ticket availability, handles countdowns.
    * RedPointNotification - Everything under RedPointNotification defines a simple model/view system for showing and managing “red dot” notification indicators in the UI.
    * SafeArea - It provides components/events for adjusting UI canvases to device “safe areas” (and handling size changes) on phones/tablets.
    * Timer - It implements reusable timer classes (with localization support and auto‑pause) plus a helper for injecting parameters into localized strings.
    * Tooltips - It provides a mini framework for showing, localizing, and managing tooltip views/events in the UI.
    * AnimatorsPeriodicTrigger – fires an initial trigger on a list of animators then repeatedly sends a periodic trigger at set intervals.
    * ButtonLongPress – a Button subclass that detects long‑press (with optional repeat) and exposes related UnityEvents.
    * ButtonLongPressCountChanger – watches a ButtonLongPress and invokes an event with a value that steps through a configured list on each long‑press.
    * ButtonLongPressToggle – extends ButtonLongPress into a toggle button, flipping state on long‑press and updating its visuals.
    * ClickNotInteractable – fires an event when the user clicks a Selectable that wasn’t interactable.
    * ClickSound – plays configurable UI sounds on pointer down/click (including disabled clicks) and hooks into long‑press buttons.
    * DragEventInterrupter – consumes all drag events on a GameObject so nothing else receives them.
    * Dropdown – custom dropdown UI that manages toggles as options and closes on outside clicks.
    * IUISoundPlayer – interface defining a method to play a named UI sound.
    * LoadingOverlay – shows/hides an animated overlay with optional input lock and exposes show/hide events.
    * ProgressBarWithTween – smoothly tweens a slider’s value with text & optional animation triggers.
    * RectInBounds – utility that calculates offset required to keep one bounds rectangle inside another.
    * ScrollExtensions – extension methods for ScrollRect to focus items/points (with alignment & animation).
    * ScrollRectFocusOnElement – component wrapping scroll‑rect focus calls with animation and threshold checks.
    * ToggleGroupPublicToggles – subclass of ToggleGroup that exposes its toggles collection publicly.
    * UIParticleScaler – scales particle systems inside a UI rect when the rect size changes (with editor bake support).
* **Changed** -
* **Fixed** -