// Design tokens — "Deep Green + Cream" palette. Light only; dark mode deferred.
// Plain module (no context/provider) to match the app's lightweight style.

export const colors = {
  primary: '#2F5D50',
  primaryPressed: '#274E43',
  primaryDisabled: '#9DB5AD',   // muted tint for disabled buttons (was #a5b4fc)
  secondary: '#5E8577',         // softened secondary green (was #6F8F82)
  accent: '#C89B5C',            // gold — details / active icons / borders only

  background: '#F7F3EA',
  card: '#FFFFFF',

  text: '#242424',              // was #1a1a1a
  textSecondary: '#5F5B54',     // darkened for WCAG AA on cream (was #888)
  textTertiary: '#77736B',      // long descriptions (was #444 / #555)

  border: '#E2DCCC',            // warm border on cream (was #ddd)
  track: '#ECE7DC',             // toggle / tab background (was #eee)

  danger: '#EF4444',            // semantic — unchanged
  overlay: 'rgba(0,0,0,0.4)',
  onPrimary: '#FFFFFF',
};

export const radius = { sm: 8, md: 10, lg: 14, xl: 20 };

export const shadow = {
  card: {
    shadowColor: '#000',
    shadowOpacity: 0.06,
    shadowRadius: 8,
    shadowOffset: { width: 0, height: 2 },
    elevation: 3,
  },
};
