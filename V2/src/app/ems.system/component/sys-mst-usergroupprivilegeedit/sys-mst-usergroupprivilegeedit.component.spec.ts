import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SysMstUsergroupprivilegeeditComponent } from './sys-mst-usergroupprivilegeedit.component';

describe('SysMstUsergroupprivilegeeditComponent', () => {
  let component: SysMstUsergroupprivilegeeditComponent;
  let fixture: ComponentFixture<SysMstUsergroupprivilegeeditComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SysMstUsergroupprivilegeeditComponent]
    });
    fixture = TestBed.createComponent(SysMstUsergroupprivilegeeditComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
