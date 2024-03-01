<script setup>
    import router from '@/router.js'
    import { useAuth } from '@/api/auth.js'
    import { ref, onMounted, onUnmounted } from 'vue'

    import HouseIcon from '@/icons/HouseIcon.vue'
    import PersonIcon from '@/icons/PersonIcon.vue'
    import CashIcon from '../icons/CashIcon.vue'
    import MovementIcon from '../icons/MovementIcon.vue'
    import PiggyBank from '../icons/PiggyBank.vue'

    const { isLoggedIn } = useAuth();

    const minWidthBeforeResizeLarge = 1000;
    const minWidthBeforeResizeSmall= 650;
    const smallSize = 70;
    const midWidth = 190;
    const largeWidth = 290;

    const navWidth = ref(largeWidth);
    const isTrimmed = ref(false);

    function onWindowResized(){
        if(window.innerWidth < minWidthBeforeResizeSmall){
            navWidth.value = smallSize
            isTrimmed.value = true;
        } else if(window.innerWidth < minWidthBeforeResizeLarge) {
            navWidth.value = midWidth
            isTrimmed.value = false;
        } else  {
            navWidth.value = largeWidth 
            isTrimmed.value = false;
        }
    }

    onMounted(()=>{
        window.addEventListener("resize", onWindowResized);
        onWindowResized();
    })  
    
    onUnmounted(()=>{
        window.removeEventListener("resize", onWindowResized);
    })

</script>

<template>
    <nav class="bm-navbar">
        <div :class="isTrimmed ? 'p-3': 'p-4'">
            <h1> 
                <RouterLink v-if="!isTrimmed" to="/" class="title">FM</RouterLink>
                <RouterLink v-else to="/" class="title title-small">FM</RouterLink>
                <span v-if="!isTrimmed">Finanzen Manager</span>
            </h1>
            <ul class="list-unstyled mb-5" style="padding: 0px;">
                <li>
                    <RouterLink to="/" class="navlink">
                        <span v-if="isTrimmed"> </span>
                        <span class="mr-3"> <HouseIcon/> </span>
                        <span v-if="!isTrimmed">Home</span>
                    </RouterLink>
                </li>

                <li>
                    <RouterLink to="/Capital" class="navlink">
                        <span v-if="isTrimmed"> </span>
                        <span class="mr-3"> <CashIcon/> </span>
                        <span v-if="!isTrimmed">Kapital</span>
                    </RouterLink>
                </li>

                <li>
                    <RouterLink to="/Transaction" class="navlink">
                        <span v-if="isTrimmed"> </span>
                        <span class="mr-3"> <MovementIcon/> </span>
                        <span v-if="!isTrimmed">Transaktionen</span>
                    </RouterLink>
                </li>

                <li>
                    <RouterLink to="/Saving" class="navlink">
                        <span v-if="isTrimmed"> </span>
                        <span class="mr-3"> <PiggyBank/> </span>
                        <span v-if="!isTrimmed">Sparziel</span>
                    </RouterLink>
                </li>

                <li v-if="!isLoggedIn">
                    <RouterLink to="/login" class="navlink">
                        <span v-if="isTrimmed"> </span>
                        <span class="mr-3"> <PersonIcon/> </span>
                        <span v-if="!isTrimmed">Login</span>
                    </RouterLink>
                </li>
                <li v-else>
                    <RouterLink to="/account" class="navlink">
                        <span v-if="isTrimmed"> </span>
                        <span class="mr-3"> <PersonIcon/> </span>
                        <span v-if="!isTrimmed">Account</span>
                    </RouterLink>
                </li >

            </ul>
        </div>
    </nav>
</template>

<style scoped>
    .bm-navbar {
        position: relative;
        background-image: linear-gradient(45deg, rgba(79, 198, 222, 0.8), #cf4fdbcc),
            url(https://colorlib.com/etc/bootstrap-sidebar/sidebar-06/images/bg_1.jpg);

        background-size: cover;
        background-repeat: no-repeat;
        background-position: center center;

        min-width: calc(1px * v-bind(navWidth));
        max-width: calc(1px * v-bind(navWidth));

        transition: 300ms;
    }

    .bm-navbar h1 {
        margin-bottom: 20px;
        font-weight: 700;
        font-size: 30px;
    }

    .bm-navbar h1 span {
        font-size: 13px;
        color: #fff;
        display: block;
    }

    .title {
        color: #fff;
        text-decoration: none;
    }

    .title-small {
        font-size: 23px;
    }

    .bm-navbar ul li {
        font-size: 16px;
        padding-top: 10px;
        padding-bottom: 10px;

        border-bottom: solid 1px rgba(255, 255, 255, 0.323);
    }
    .navlink {
        color: #ffffffad;
        text-decoration: none;

        transition: 200ms;
    }

    .navlink:hover {
        color: white;
    }

    .navlink span {
        margin-right: 10px;
        color: #fff;
    }

</style>