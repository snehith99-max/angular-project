import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SysMstUsermanagementSummaryComponent } from './sys-mst-usermanagement-summary.component';

describe('SysMstUsermanagementSummaryComponent', () => {
  let component: SysMstUsermanagementSummaryComponent;
  let fixture: ComponentFixture<SysMstUsermanagementSummaryComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SysMstUsermanagementSummaryComponent]
    });
    fixture = TestBed.createComponent(SysMstUsermanagementSummaryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
