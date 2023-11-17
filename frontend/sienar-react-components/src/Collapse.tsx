import type { HTMLAttributes, PropsWithChildren } from 'react';
import { classNames, cleanProps } from '@/utils';

type CollapseProps = {
	open: boolean
} & PropsWithChildren & HTMLAttributes<HTMLElement>;

export default function Collapse(props: CollapseProps) {
	const { open } = props;

	const cleanedProps = cleanProps(props, 'open') as HTMLAttributes<HTMLElement>;

	cleanedProps.className = classNames(
		'collapse',
		{
			'collapse--open': open
		}
	);

	return <div {...cleanedProps}/>;
}