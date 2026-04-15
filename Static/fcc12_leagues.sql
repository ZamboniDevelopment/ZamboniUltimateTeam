--
-- PostgreSQL database dump
--

\restrict j8fLj3I4xThju1pDLtiIjnSAmhb19Da9u8bkjQ0zasbgfOCyvlGRLb5xfkj92E3

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
-- Name: fcc12_leagues; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.fcc12_leagues (
    teamid integer,
    leagueid integer
);


ALTER TABLE public.fcc12_leagues OWNER TO postgres;

--
-- Data for Name: fcc12_leagues; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.fcc12_leagues (teamid, leagueid) FROM stdin;
\.


--
-- PostgreSQL database dump complete
--

\unrestrict j8fLj3I4xThju1pDLtiIjnSAmhb19Da9u8bkjQ0zasbgfOCyvlGRLb5xfkj92E3

