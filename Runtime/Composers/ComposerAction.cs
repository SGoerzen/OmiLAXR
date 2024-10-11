namespace OmiLAXR.Composers
{
    public delegate void ComposerAction(Composer sender);
    public delegate void ComposerAction<in T>(Composer sender, T obj);
    public delegate void ComposerAction<in T1, in T2>(Composer sender, T1 obj1, T2 obj2);
    public delegate void ComposerAction<in T1, in T2, in T3>(Composer sender, T1 obj1, T2 obj2, T3 obj3);
    public delegate void ComposerAction<in T1, in T2, in T3, in T4>(Composer sender, T1 obj1, T2 obj2, T3 obj3, T4 obj4);
    public delegate void ComposerAction<in T1, in T2, in T3, in T4, in T5>(Composer sender, T1 obj1, T2 obj2, T3 obj3, T4 obj4, T5 obj5);
    public delegate void ComposerAction<in T1, in T2, in T3, in T4, in T5, in T6>(Composer sender, T1 obj1, T2 obj2, T3 obj3, T4 obj4, T5 obj5, T6 obj6);
}