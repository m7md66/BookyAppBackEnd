# Expo HAS CHANGED

Read the exact versioned docs at https://docs.expo.dev/versions/v57.0.0/ before writing any code.

## Commands

Run from this directory (`BookyAppMobile/`):

```bash
npm install
npm start        # expo start
npm run android  # expo run:android
npm run ios      # expo run:ios
npm run web      # expo start --web
```

## Architecture

Expo 54 / React Native 0.81 / React 19 client for the BookyApp ASP.NET Core backend at `../BookyApp` (see its own `CLAUDE.md`).

- **State** — `zustand`. Only auth state (`token`, `user`, `needsInterests`) is global, in [src/store/authStore.js](src/store/authStore.js). Screen-local state uses plain `useState`.
- **Navigation** — `@react-navigation`. [src/navigation/index.js](src/navigation/index.js) is the root switcher: it picks `AuthNavigator`, `InterestsScreen`, or `MainNavigator` based on `authStore`'s `token` / `needsInterests`. [src/navigation/MainNavigator.js](src/navigation/MainNavigator.js) nests a bottom-tab navigator (Feed / Browse / MyLibrary) inside a stack that also holds `ReadBookScreen`, so reading a book pushes over the tabs instead of replacing them.
- **API** — one file per backend controller under `src/api/` (`auth.js`, `books.js`, `quotations.js`, `interests.js`, `files.js`), all built on the shared axios instance in [src/api/client.js](src/api/client.js). `BASE_URL` is hardcoded to `http://10.0.2.2:5212/api` (Android emulator's loopback to the host machine's `localhost`, matching the backend's default Kestrel port) — change it to your LAN IP for a physical device or the iOS simulator.
- **Auth** — JWT bearer token stored in `AsyncStorage` under key `'token'`, attached automatically by the `client.js` request interceptor. `authStore.loadToken()` rehydrates it on app start before the navigator renders.

### Backend response-shape gotchas
- List/paged endpoints (`GetFavoriteBooks`, `GetBrowseBooks`, `GetFeed`, ...) return the payload under `res.data.dataResult`.
- Login returns the token alongside user fields under `res.data.data` (fall back to `res.data` if `data` is absent).
- `Account/AddUser` (register) always responds `200` even on failure (weak password, duplicate email, etc.) — check `res.data.isSuccess` before treating it as success, and read the message from `res.data.validationErrors[0].description`.
- `AddUser` doesn't return a token, so registration immediately calls `login()` to authenticate the new session.

## Screens & components
- `src/screens/auth/` — `LoginScreen`, `RegisterScreen`, `InterestsScreen` (post-register genre picker; gates entry to `MainNavigator` via `needsInterests`).
- `src/screens/main/` — `FeedScreen`, `BrowseBooksScreen`, `MyLibraryScreen`, `ReadBookScreen` (renders the book via a `WebView` + [pdfViewerHtml.js](src/screens/main/pdfViewerHtml.js); the in-page viewer posts `{type: 'quote', text}` messages back to RN, which calls `createQuotation`).
- `src/components/` — `BookCard`, `BookCoverPlaceholder`, `QuotationCard`.

## Related
Backend API lives at `../BookyApp` — see its `CLAUDE.md` for controllers, DTOs, and entities. When adding a screen or API call here, check the actual DTO/controller shape there rather than guessing field names or casing.
