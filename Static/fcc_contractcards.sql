--
-- PostgreSQL database dump
--

\restrict 27DpeVYGJZ13b0703KNxZIW6KyzHpGaB9Sbg7NsjU1mbS82c5YcrBRviSNxLXHm

-- Dumped from database version 18.4
-- Dumped by pg_dump version 18.4

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET transaction_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

SET default_tablespace = '';

SET default_table_access_method = heap;

--
-- Name: fcc_contractcards; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.fcc_contractcards (
    carddbid integer,
    weightrare integer,
    value integer,
    zcat integer DEFAULT 0
);


ALTER TABLE public.fcc_contractcards OWNER TO postgres;

--
-- Data for Name: fcc_contractcards; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.fcc_contractcards (carddbid, weightrare, value, zcat) FROM stdin;
5001001	0	8	0
5001002	0	9	0
5001003	0	10	0
5001004	0	11	0
5001005	0	13	0
5001006	0	15	0
5001007	2000	25	0
5001008	1000	30	0
5001009	800	35	0
5001010	40	40	0
5001011	20	80	0
\.


--
-- PostgreSQL database dump complete
--

\unrestrict 27DpeVYGJZ13b0703KNxZIW6KyzHpGaB9Sbg7NsjU1mbS82c5YcrBRviSNxLXHm

