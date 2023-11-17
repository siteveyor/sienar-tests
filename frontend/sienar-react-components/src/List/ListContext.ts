import { createContext } from 'react';
import type { Themeable } from '@/types';

type ListContextType = Themeable;

export const ListContext = createContext<ListContextType>({});