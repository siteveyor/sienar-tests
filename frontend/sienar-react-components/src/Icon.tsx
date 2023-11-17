import { classNames, cleanProps } from '@/utils';
import type { HTMLAttributes } from 'react';

export type IconProps = {
	iconStyle?: 'solid' | 'regular' | 'light' | 'duotone' | 'thin' | 'brands'
	family?: 'classic' | 'sharp'
	rotation?: 'none' | '90' | '180' | '270'
	flip?: 'none' | 'horizontal' | 'vertical' | 'both'
	size?: '2xs' | 'xs' | 'sm' | 'md' | 'lg' | 'xl' | '2xl'
	icon: string
} & Omit<HTMLAttributes<HTMLElement>, 'children'>

export default function Icon(props: IconProps) {
	const {
		iconStyle = 'solid',
		family,
		rotation,
		flip,
		size,
		icon
	} = props;

	const cleanedProps = cleanProps(props, 'icon', 'iconStyle', 'family', 'rotation', 'flip', 'size') as HTMLAttributes<HTMLElement>;

	const dynamicClasses: Record<string, boolean> = {
		'fa-sharp': family === 'sharp'
	};
	dynamicClasses[`fa-rotate-${rotation}`] = !!rotation && rotation !== 'none';
	dynamicClasses[`fa-flip-${flip}`] = !!flip && flip !== 'none';
	dynamicClasses[`fa-${size}`] = !!size && size !== 'md';

	cleanedProps.className = classNames(
		`fa-${iconStyle}`,
		`fa-${icon}`,
		dynamicClasses
	);

	return <i {...cleanedProps}/>;
}