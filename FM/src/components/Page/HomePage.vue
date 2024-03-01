<script setup>
import CapitalVisual from '../CapitalVisual.vue';
import TransactionVisual from '../TransactionVisual.vue';
import SavingVisual from '../SavingVisual.vue';

    
</script>

<template>
    <section class="p-2">
        <CapitalVisual class="element" />
        <TransactionVisual  class="element" />
        <SavingVisual class="element" />
    </section>
</template>

<style>
    .element {
        margin-bottom: 12px;
    }
</style>