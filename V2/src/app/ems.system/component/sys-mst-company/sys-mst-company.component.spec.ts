import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SysMstCompanyComponent } from './sys-mst-company.component';

describe('SysMstCompanyComponent', () => {
  let component: SysMstCompanyComponent;
  let fixture: ComponentFixture<SysMstCompanyComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SysMstCompanyComponent]
    });
    fixture = TestBed.createComponent(SysMstCompanyComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
