import { classNames, cleanProps } from '@/utils';
import { RefObject, useEffect, useRef } from 'react';

import type { AnchorDirection, SemanticContainer } from '@/types';
import type { HTMLAttributes, PropsWithChildren } from 'react';

type DrawerProps = {
	tag?: SemanticContainer|'div'
	anchor?: AnchorDirection
	permanent?: boolean
	full?: boolean
	open?: boolean
	onClose?: () => any
} & PropsWithChildren & HTMLAttributes<HTMLElement>;

export default function Drawer(props: DrawerProps) {
	const {
		anchor = 'left',
		permanent = false,
		full = false,
		open = false,
		onClose
	} = props;
	const Tag = props.tag ?? 'div';
	const ref = useRef<HTMLElement>(null);

	const handleClose = (event: MouseEvent) => {
		if (permanent) {
			return;
		}

		if (!open) {
			return;
		}

		if (!ref.current?.contains(event.target as Node)) {
			onClose?.();
		}
	};

	useEffect(() => {
		document.body.addEventListener('click', handleClose);

		return () => document.body.removeEventListener('click', handleClose);
	}, [])

	const cleanedProps = cleanProps(props, 'anchor', 'permanent', 'full', 'tag', 'onClose') as HTMLAttributes<HTMLElement>;

	cleanedProps.className = classNames(
		'drawer',
		`drawer--${anchor}`,
		{
			'drawer--permanent': permanent,
			'drawer--full': full,
			'drawer--open': permanent ? false : open
		}
	)

	return (
		<Tag
			ref={ref as RefObject<HTMLDivElement>} // *sigh*
			{...cleanedProps}
		/>
	);
}