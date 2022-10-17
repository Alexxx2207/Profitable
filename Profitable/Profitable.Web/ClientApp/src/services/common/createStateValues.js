

export const changeStateValuesForControlledForms = (values, inputName, inputValue) =>{
    return {
        ...values,
        [inputName]: inputValue
    }
}

export const changeStateValuesForControlledFormsByTrimming = (values, inputName, inputValue) =>{
    return {
        ...values,
        [inputName]: inputValue.trim()
    }
}