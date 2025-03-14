"use client";

type Props = {
    className: string;
    children: React.ReactNode;
}

export default function PlayNowButton({ className, children  }: Props) {
    const onPlayNowClick = () => {
        //router.push("/games/1");
    };

    return <button className={className} onClick={onPlayNowClick}>{children}</button>;
}
