select * from
(
  select rtrim(S.IDAPP) as IDAPP,rtrim(A.KDAPP) as KDAPP,rtrim(A.NMAPP) as NMAPP,rtrim(A.URAPP) as URAPP,
  rtrim(S.KDMENU) as KDMENU,rtrim(S.NMMENU) as NMMENU,
  rtrim(S.IDMENU) as IDMENU,rtrim(S.KDDOK) as KDDOK,rtrim(S.IDXDOK) as IDXDOK,
  rtrim(S.URL) as URL,
  rtrim(S.URL)+'app='+S.IDAPP+'&amp;id='+IDMENU as URLFULL,
  rtrim(S.OLIST1) as OLIST1,rtrim(S.OLISTDETIL1) as OLISTDETIL1,rtrim(S.OLISTDETIL2) as OLISTDETIL2,
  rtrim(S.OLISTDETIL3) as OLISTDETIL3,rtrim(S.OLISTDETIL4) as OLISTDETIL4,
  S.STATUS,S.KDLEVEL,rtrim(S.TYPE) as TYPE,rtrim(isnull(S.LAST_BY,'')) as LAST_BY,S.LAST_DATE
  from SS00APPMENU S
  inner join SS00APP A on S.IDAPP=A.IDAPP
  union
  select rtrim(S.IDAPP) as IDAPP,rtrim(A.KDAPP) as KDAPP,rtrim(A.NMAPP) as NMAPP,rtrim(A.URAPP) as URAPP,
  rtrim(S.KDMENU) as KDMENU,rtrim(S.NMMENU) as NMMENU,
  rtrim(S.IDMENU) as IDMENU,rtrim(S.KDDOK) as KDDOK,rtrim(S.IDXDOK) as IDXDOK,
  rtrim(S.URL) as URL,
  rtrim(S.URL)+'app='+S.IDAPP+'&amp;id='+IDMENU as URLFULL,
  rtrim(S.OLIST1) as OLIST1,rtrim(S.OLISTDETIL1) as OLISTDETIL1,rtrim(S.OLISTDETIL2) as OLISTDETIL2,
  rtrim(S.OLISTDETIL3) as OLISTDETIL3,rtrim(S.OLISTDETIL4) as OLISTDETIL4,
  S.STATUS,S.KDLEVEL,rtrim(S.TYPE) as TYPE,rtrim(isnull(S.LAST_BY,'')) as LAST_BY,S.LAST_DATE
  from SS01APPMENU S
  inner join SS00APP A on S.IDAPP=A.IDAPP
)S

