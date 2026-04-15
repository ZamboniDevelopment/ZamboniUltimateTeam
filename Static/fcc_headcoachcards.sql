--
-- PostgreSQL database dump
--

\restrict p4O2acpLEchQyh3NF8eyU0lmkqNNU2PB1x7ZSGk1kaUl5G04i9s1fPQX4k9VbCd

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
-- Name: fcc_headcoachcards; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.fcc_headcoachcards (
    carddbid integer,
    attribute integer,
    assetid integer,
    firstname character varying,
    lastname character varying,
    value integer,
    amount integer,
    rare boolean
);


ALTER TABLE public.fcc_headcoachcards OWNER TO postgres;

--
-- Data for Name: fcc_headcoachcards; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.fcc_headcoachcards (carddbid, attribute, assetid, firstname, lastname, value, amount, rare) FROM stdin;
2000000	1	0	DAMIEN	SMITH	75	4	f
2000001	2	1	BOB	ADAMS	75	32	f
2000002	4	2	CHRIS	MATTHEWS	75	256	f
2000003	8	3	DENNIS	JONES	75	2048	f
2000004	16	4	CRAIG	DANIELS	75	16384	f
2000005	32	5	JOHN	WILLIAMS	75	131072	f
2000006	64	6	JUSTIN	DAVIS	75	1048576	f
2000007	128	7	AVERY	JAMES	75	8388608	f
2000008	256	8	ELLIOTT	JOHNSON	75	67108864	f
2000009	512	9	ANDY	MCGEE	75	536870912	f
2000010	3	10	ISAAC	MACARTHUR	75	36	f
2000011	6	11	SEAN	KINGSLEY	75	344	f
2000012	12	12	RAYMOND	KRAUSE	75	2304	f
2000013	17	13	JASON	MCKAY	75	16388	f
2000014	96	14	JAMES	RUPERT	75	1179648	f
2000015	640	15	DEREK	CRAIG	75	413138944	f
2000016	19	16	MICHAEL	DAVIDSON	75	16420	t
2000017	14	17	BRAD	ROME	75	2840	t
2000018	25	18	JAY	REAGAN	75	6661	t
2000019	416	19	ROBERT	NORRIS	75	67239936	t
2000020	15	20	BRIAN	FIDDLER	75	2276	t
2000021	27	21	IGOR	SKUIN	75	2604	t
2000022	30	22	BORIS	IVANSKY	75	22296	t
2000023	480	23	GREG	RICHARDS	75	58064896	t
2000024	31	24	DEAN	SAMUELS	75	18724	t
2000025	992	25	MARK	ABBOTT	75	613548032	t
\.


--
-- PostgreSQL database dump complete
--

\unrestrict p4O2acpLEchQyh3NF8eyU0lmkqNNU2PB1x7ZSGk1kaUl5G04i9s1fPQX4k9VbCd

