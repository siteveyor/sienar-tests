import type { Blank, ThemeColor, ThemeVariant } from '@/types';

export function createThemedClassNames(
	color: ThemeColor|Blank,
	variant: ThemeVariant|Blank,
	baseClassName: string
): string {
	return classNames(
		baseClassName,
		{
			[`${baseClassName}--${color}`]: !!color,
			[`${baseClassName}--${variant}`]: !!variant
		}
	);
}

export function classNames(...args: (string|Record<string, boolean>)[]): string {
	const classes: string[]= [];

	args.forEach(a => {
		if (typeof a === 'string') {
			classes.push(a);
			return;
		}

		Object.keys(a).forEach(k => {
			if (a[k]) {
				classes.push(k);
			}
		});
	});

	return classes.join(' ');
}

export function cleanThemeFromProps(props: Object, ...extraNamesToRemove: string[]): Object {
	return cleanProps(props, 'color', 'variant', ...extraNamesToRemove);
}

export function cleanProps(props: Object, ...namesToRemove: string[]): Object {
	const cleaned: Record<string, any> = Object.assign({}, props);
	namesToRemove.forEach(n => delete cleaned[n]);
	return cleaned;
}