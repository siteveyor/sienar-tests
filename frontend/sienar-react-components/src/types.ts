export type ThemeColor = 
	| 'primary'
	| 'secondary'
	| 'tertiary'
	| 'success'
	| 'error'
	| 'warning'
	| 'info'
	| 'light'
	| 'dark'
	| 'white'
	| 'black'
	| 'default';

export type ThemeVariant =
	| 'solid'
	| 'outlined'
	| 'text';

export type Themeable = {
	color?: ThemeColor
	variant?: ThemeVariant
}

export type Blank = null|undefined;

export type SemanticContainer = 
	| 'aside'
	| 'nav'
	| 'section'
	| 'article'
	| 'footer'
	| 'header'
	| 'main';

export type AnchorDirection = 
	| 'left'
	| 'right'
	| 'top'
	| 'bottom';

export type FormValueValidator<T extends unknown> = {
	(input: T): string|null
}

export type FormFieldValidator = {
	(): boolean
}