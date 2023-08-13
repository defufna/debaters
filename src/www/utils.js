export function toBase62(n) {
    const characters = '0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ';
    const base = BigInt(characters.length);
    let result = '';

    if (n === 0n) {
      return characters[0];
    }

    while (n > 0) {
      result = characters[n % base] + result;
      n = n / base;
    }

    return result;
}

export function fromBase62(s) {
    const characters = '0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ';
    const charMap = {};
    for (let i = 0; i < characters.length; i++) {
        charMap[characters[i]] = BigInt(i);
    }

    const base = BigInt(characters.length);
    let result = 0n;

    for (let i = 0; i < s.length; i++) {
        const char = s.charAt(i);
        const charValue = charMap[char];
        result = result * base + charValue;
    }

    return result;
}