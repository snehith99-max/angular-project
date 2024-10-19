import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SysMstRevenuecategorysummaryComponent } from './sys-mst-revenuecategorysummary.component';

describe('SysMstRevenuecategorysummaryComponent', () => {
  let component: SysMstRevenuecategorysummaryComponent;
  let fixture: ComponentFixture<SysMstRevenuecategorysummaryComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SysMstRevenuecategorysummaryComponent]
    });
    fixture = TestBed.createComponent(SysMstRevenuecategorysummaryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
