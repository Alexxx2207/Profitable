export const naturalNumberChecker = (value) => {
    return value > 0;
};

export const minLengthChecker = (value, bound) => {
    return value?.trim().length >= bound;
};

export const maxLengthChecker = (value, bound) => {
    return value?.trim().length <= bound;
};

export const isEmptyOrWhiteSpaceFieldChecker = (value) => {
    return !!value.trim();
};

export const isWhiteSpaceFieldChecker = (value) => {
    return !!value.replace(/\s/g, "").length;
};

export const isEmailValidChecker = (email) => {
    const re =
        // eslint-disable-next-line
        /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
    return re.test(String(email.trim()).toLowerCase());
};

export const isPasswordValidChecker = (email) => {
    // eslint-disable-next-line
    const re = /^[\w@!#$%^&*()_+\-={\[}\];:'"\\\|,<.>\/?`\~]+$/;
    return re.test(String(email.trim()).toLowerCase());
};
