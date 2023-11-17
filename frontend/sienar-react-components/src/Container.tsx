import { classNames, cleanProps } from '@/utils';
import type { HTMLAttributes, PropsWithChildren } from 'react';

type ContainerProps = {
	fixed?: boolean
	maxWidth?: 'sm'|'md'|'lg'|'xl'|'xxl'|false
} & PropsWithChildren & HTMLAttributes<HTMLElement>

export default function Container(props: ContainerProps) {
	const {
		fixed = false,
		maxWidth = false
	} = props;

	const cleanedProps = cleanProps(props, 'fixed','maxWidth') as HTMLAttributes<HTMLElement>;

	// noinspection PointlessBooleanExpressionJS
	cleanedProps.className = classNames(
		'container',
		{
			[`container--${maxWidth}`]: !!maxWidth,
			'container--fixed': fixed
		}
	);

	return <div {...cleanedProps}/>;
}