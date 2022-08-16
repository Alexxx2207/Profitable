

export const changeStateValuesForControlledForms = (values, inputName, inputValue) =>{
    return {
        ...values,
        [inputName]: inputValue.trim()
    }
}