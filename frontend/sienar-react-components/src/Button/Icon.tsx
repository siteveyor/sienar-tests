import Button from './Button';
import Icon from '@/Icon';
import { cleanThemeFromProps } from '@/utils';

import type { ButtonProps } from './Button';
import type { IconProps } from '@/Icon';

type IconButtonProps = ButtonProps & IconProps;

export default function IconButton(props: IconButtonProps) {
	const {
		color = 'default',
		variant = 'text',
		iconStyle,
		family,
		rotation,
		flip,
		size,
		icon
	} = props;

	const cleanedProps = cleanThemeFromProps(props, 'iconStyle', 'family', 'rotation', 'flip', 'size', 'icon') as ButtonProps;

	return (
		<Button 
			color={color}
			variant={variant}
			{...cleanedProps}
		>
			<Icon
				iconStyle={iconStyle}
				family={family}
				rotation={rotation}
				flip={flip}
				size={size}
				icon={icon}
			/>
		</Button>
	)
}