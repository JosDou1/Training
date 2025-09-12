export function getMaxKeyLength<K>(map: Map<K, unknown>): number {
    let max = 0;

    for (const key of map.keys()) {
        const len = String(key).length;
        if (len > max) {
            max = len;
        }
    }

    return max;
}

 export function formatPounds(amount: number): string {
    const isNegative = amount < 0;
    const absAmount = Math.abs(amount);

    const formatted = absAmount.toFixed(2);

    return isNegative ? `-£${formatted}` : ` £${formatted}`;
}