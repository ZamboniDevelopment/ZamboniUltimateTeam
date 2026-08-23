--
-- PostgreSQL database dump
--

\restrict BhAaH0SOqPUgWTmoVRJnI0nkL5aOpIThWZuq1fEuLIgdvv29lM8fXHDlermna9t

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
-- Name: fcc_stadium; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.fcc_stadium (
    carddbid integer,
    capacity integer,
    biodescription character varying,
    weightrare integer,
    cardassetid integer,
    description character varying,
    assetid integer,
    stadiumid integer,
    value integer,
    arenatype integer,
    name character varying,
    category integer,
    header character varying,
    zcat integer DEFAULT 0
);


ALTER TABLE public.fcc_stadium OWNER TO postgres;

--
-- Data for Name: fcc_stadium; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.fcc_stadium (carddbid, capacity, biodescription, weightrare, cardassetid, description, assetid, stadiumid, value, arenatype, name, category, header, zcat) FROM stdin;
6200000	50	StadiumDetailDesc	0	0	StadiumDesc_1	0	0	80	0	EA SPORTS ™ Arena1	4	Stadium	0
6200001	75	StadiumDetailDesc	0	0	StadiumDesc_4	1	0	80	0	EA SPORTS ™ Arena4	4	Stadium	0
6200002	100	StadiumDetailDesc	10	0	StadiumDesc_5	2	0	80	0	EA SPORTS ™ Arena5	4	Stadium	0
6200003	50	StadiumDetailDesc	0	1	StadiumDesc_6	3	0	80	1	EA SPORTS ™ Arena6	4	Stadium	0
6200004	75	StadiumDetailDesc	0	1	StadiumDesc_12	4	0	80	1	EA SPORTS ™ Arena12	4	Stadium	0
6200005	100	StadiumDetailDesc	10	1	StadiumDesc_13	5	0	80	1	EA SPORTS ™ Arena13	4	Stadium	0
\.


--
-- PostgreSQL database dump complete
--

\unrestrict BhAaH0SOqPUgWTmoVRJnI0nkL5aOpIThWZuq1fEuLIgdvv29lM8fXHDlermna9t

