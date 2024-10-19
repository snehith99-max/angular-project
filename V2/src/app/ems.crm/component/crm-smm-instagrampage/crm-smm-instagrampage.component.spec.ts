import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CrmSmmInstagrampageComponent } from './crm-smm-instagrampage.component';

describe('CrmSmmInstagrampageComponent', () => {
  let component: CrmSmmInstagrampageComponent;
  let fixture: ComponentFixture<CrmSmmInstagrampageComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [CrmSmmInstagrampageComponent]
    });
    fixture = TestBed.createComponent(CrmSmmInstagrampageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
