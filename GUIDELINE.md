# Guideline

- **Do** create a module for each component. The module should be named the same as the component and be placed in the same folder level.
- **Avoid** creating dependencies with parent modules of sibling modules.
- **Do** use Sass variables to avoid repetitions (see: `arrow.component.scss`).
- **Do** create a theme file prefixed by `theme.scss` (e.g. `button.theme.scss`) for all component styles related to the theme (i.e. colors and typography). That file should be placed next to the components files.
- **Do** create a theme file in each module containing sub-modules with their own theme file.
  <!-- Theme namespaces? -->
- **Do** add custom paths in `tsconfig.json` to simplify imports (e.g. `~env`, `~core`, `~shared`).
