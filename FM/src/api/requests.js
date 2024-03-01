import { useAuth } from './auth'

const backend = '/api'

const { token, setToken } = useAuth()

export async function fetchSavings(){
    const response = await request('/savings');
    return response;
}

export async function fetchTotalSavings(){
    const response = await request('/totalsavings');
    return response;
}

export async function createSaving(savingName, savingGoal, monthlyAmount, startDate){
    const id = -1;

    const response = await request(`/saving`, {
        method: 'POST',
        body: JSON.stringify({id, savingName, savingGoal, monthlyAmount, startDate }),
    })

    return response
}

export async function deleteSaving(savingId){
    const response = await request(`/saving`, {
        method: 'DELETE',
        body: JSON.stringify({savingId}),
    })

    return response
}


export async function fetchTransactions(){
    const response = await request('/transactions');
    return response;
}

export async function fetchTotalEarned(){
    const response = await request('/totalearned');
    return response;
}

export async function fetchTotalLost(){
    const response = await request('/totallost');
    return response;
}

export async function createTransaction(transactionName, transactionValue, startDate, endDate){
    const id = -1;

    const response = await request(`/transaction`, {
        method: 'POST',
        body: JSON.stringify({ id, transactionName, transactionValue, startDate, endDate }),
    })

    return response
}

export async function deleteTransaction(transactionId){
    const response = await request(`/transaction`, {
        method: 'DELETE',
        body: JSON.stringify({ transactionId }),
    })

    return response
}


export async function defineCapital(capital, capitalDate){
    const response = await request(`/capital`, {
        method: 'POST',
        body: JSON.stringify({ capital, capitalDate }),
    })

    return response
}

export async function fetchCapital(){
    const response = await request('/capital');
    return response;
}


export async function fetchUserData(){
    const response = await request(`/user`);
    return response;
}

export async function registerUser (username, password, email){
    const response = await request(`/register`, {
        method: 'POST',
        body: JSON.stringify({ username, password, email }),
    })
}

export async function loginUser (username, password) {
    const response = await request(`/login`, {
        method: 'POST',
        body: JSON.stringify({ username, password }),
    })

    if (!response.token) {
        throw new Error('Login failed due to an unknown error')
    }

    setToken(response.token)

    return response.token
}

export async function checkAuth () {
    try {
        const response = await request(`/auth`, {
            method: 'GET',
        })

        if (response.token) {
            setToken(response.token)
        }

        return response
    } catch (error) {
        setToken('')
        return false
    }
}

async function request(url, options = {}) {
    const headers = {
        'Content-Type': 'application/json',
        'X-Requested-With': 'XMLHttpRequest',
    }

    if (token.value) {
        headers['Authorization'] = 'Bearer ' + token.value
    }

    const response = await fetch(backend + url, { headers, ...options })

    if (response.ok) {
        return response.json()
    } else if (response.status === 422) {
        const data = await response.json()

        throw new ValidationError('validation failed', data.errors)
    } else {
        throw new Error(`Server error: ${await response.text()}`)
    }
}

class ValidationError {
    message
    errors

    constructor (message, errors) {
        this.message = message
        this.errors = errors
    }
}