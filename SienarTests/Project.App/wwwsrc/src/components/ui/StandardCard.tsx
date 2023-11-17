import {PropsWithChildren, ReactNode} from 'react';
import { Card } from '@sienar/react-components';
import { ThemeColor, ThemeVariant } from '@sienar/react-components/src/types';

interface Props {
	title: string
	subtitle?: string
	headerIcon?: ReactNode
	actions?: ReactNode
	color?: ThemeColor
	variant?: ThemeVariant
}

export default function StandardCard(props: PropsWithChildren<Props>) {
	const {
		actions,
		children,
		headerIcon,
		title,
		subtitle,
		color = 'primary',
		variant = 'outlined'
	} = props;

	return (
		<Card
			color={color}
			variant={variant}
			className='standard-card'
		>
			<Card.Header className='standard-card__header'>
				<div className='standard-card__title-wrapper'>
					<h4 className='standard-card__title'>{title}</h4>
					{subtitle && <p className='standard-card__subtitle'>{subtitle}</p>}
				</div>
				<div className='standard-card__icon-wrapper'>
					{headerIcon}
				</div>
			</Card.Header>

			<Card.Body className='standard-card__body'>
				{children}
			</Card.Body>

			{actions && (
				<Card.Actions className='standard-card__actions'>
					{actions}
				</Card.Actions>
			)}
		</Card>
	)
}