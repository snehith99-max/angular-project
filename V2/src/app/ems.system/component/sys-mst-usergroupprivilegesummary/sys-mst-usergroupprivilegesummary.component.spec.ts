import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SysMstUsergroupprivilegesummaryComponent } from './sys-mst-usergroupprivilegesummary.component';

describe('SysMstUsergroupprivilegesummaryComponent', () => {
  let component: SysMstUsergroupprivilegesummaryComponent;
  let fixture: ComponentFixture<SysMstUsergroupprivilegesummaryComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SysMstUsergroupprivilegesummaryComponent]
    });
    fixture = TestBed.createComponent(SysMstUsergroupprivilegesummaryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
