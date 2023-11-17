import { NavLink } from 'react-router-dom';
import { cleanThemeFromProps, createThemedClassNames } from '@/utils';

import type { AnchorHTMLAttributes, ButtonHTMLAttributes, HTMLAttributes } from 'react';
import type { NavLinkProps } from 'react-router-dom';
import type { Themeable, ThemeVariant, Blank } from '@/types';

type ButtonThemeVariant = ThemeVariant | Blank | 'icon';

export type ButtonProps = {
	variant?: ButtonThemeVariant
	suppressTheme?: boolean
} & Pick<Themeable, 'color'> & (ButtonHTMLAttributes<HTMLButtonElement> | AnchorHTMLAttributes<HTMLAnchorElement> | NavLinkProps);

export default function Button(props: ButtonProps) {
	const {
		color = 'primary',
		variant = 'solid',
		suppressTheme = false
	} = props;

	const cleanedProps = cleanThemeFromProps(props, 'suppressTheme') as HTMLAttributes<HTMLElement>;
	if (!suppressTheme) {
		cleanedProps.className = createThemedClassNames(color, variant as ThemeVariant, 'button');
	}

	if ('to' in props) {
		return <NavLink {...cleanedProps as NavLinkProps} />;
	}

	if ('href' in props) {
		return <a {...cleanedProps} />;
	}

	return <button {...cleanedProps} />;
}