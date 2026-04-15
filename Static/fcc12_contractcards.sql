--
-- PostgreSQL database dump
--

\restrict hmzYfTarZconaTSGvUGUQ4Wge0atM4WpnGSwfp8m8oZwTW9mLZYnct7dGH0kRX1

-- Dumped from database version 18.3
-- Dumped by pg_dump version 18.3

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
-- Name: fcc12_contractcards; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.fcc12_contractcards (
    carddbid integer,
    weightrare integer,
    value integer
);


ALTER TABLE public.fcc12_contractcards OWNER TO postgres;

--
-- Data for Name: fcc12_contractcards; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.fcc12_contractcards (carddbid, weightrare, value) FROM stdin;
5001001	0	8
5001002	0	9
5001003	0	10
5001004	0	11
5001005	0	13
5001006	0	15
5001007	2000	25
5001008	1000	30
5001009	800	35
5001010	40	40
5001011	20	80
\.


--
-- PostgreSQL database dump complete
--

\unrestrict hmzYfTarZconaTSGvUGUQ4Wge0atM4WpnGSwfp8m8oZwTW9mLZYnct7dGH0kRX1

