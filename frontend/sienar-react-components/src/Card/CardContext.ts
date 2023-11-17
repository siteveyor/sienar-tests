import { createContext } from 'react';
import type { Themeable } from '@/types';

type CardContextType = Themeable;

export const CardContext = createContext<CardContextType>({});