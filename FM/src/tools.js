import { formatDistance, format } from 'date-fns'
import { de } from 'date-fns/locale';

const MoneyFormatter = Intl.NumberFormat('de-DE', { style: 'currency', currency: 'CHF' });

export function formatMoney(m){
    return MoneyFormatter.format(m);
}

export function formatDate(d){
    return format(d, "dd/MM/yyyy", { locale: de} );
}

export function formatDateRelativ(d, useSuffix){
    return formatDistance(d, new Date(), { addSuffix: useSuffix, locale: de })
}


